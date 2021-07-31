using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

public static class ActionFactory
{
    private static Dictionary<string, Type> actionsByName;
    private static bool IsInitialized => actionsByName != null;

    private static void InitializeFactory()
    {
        if (IsInitialized)
        {
            return;
        }

        var allTypes = Assembly.GetAssembly(typeof(GAction)).GetTypes();
        var actionTypes = allTypes.Where(myType =>
            myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(GAction)));

        actionsByName = new Dictionary<string, Type>();

        foreach (var type in actionTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as GAction;
            actionsByName.Add(tempEffect.Name, type);
        }
    }

    public static IAction GetAction(string actionType)
    {
        InitializeFactory();

        if (actionsByName.ContainsKey(actionType))
        {
            var type = actionsByName[actionType];
            GAction action = Activator.CreateInstance(type) as GAction;
            return action;
        }

        return null;
    }

    internal static IEnumerable<string> GetActionNames() => actionsByName.Keys;

    public static List<IAction> GetActionsByEntityType(EntityType entityType)
    {
        InitializeFactory();
        var keyList = GetActionNames();
        var lib = new List<IAction>();

        foreach (var s in keyList.ToList())
        {
            IAction action = GetAction(s);

            if (action != null)
            {
                if (action.PermittedEntities.Contains(entityType))
                {
                    lib.Add(action);
                }
            }
        }

        return lib;
    }
}