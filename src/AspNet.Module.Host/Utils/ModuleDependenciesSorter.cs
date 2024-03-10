// ReSharper disable once CheckNamespace

namespace AspNet.Module.Host;

internal static class ModuleDependenciesSorter
{
    public static List<Type> TopologicalSort(Dictionary<Type, List<Type>> modulesWithDependencies)
    {
        var result = new List<Type>();

        var modulesWithDependentModules = new Dictionary<Type, List<Type>>();

        foreach (var moduleDependencies in modulesWithDependencies)
        foreach (var dependency in moduleDependencies.Value)
        {
            if (modulesWithDependentModules.ContainsKey(dependency))
            {
                modulesWithDependentModules[dependency].Add(moduleDependencies.Key);
            }
            else
            {
                modulesWithDependentModules[dependency] = new List<Type>
                {
                    moduleDependencies.Key
                };
            }
        }

        var modulesWithDependenciesCount =
            modulesWithDependencies.ToDictionary(module => module.Key, module => module.Value.Count);

        var queue = new Queue<Type>();
        foreach (var moduleWithoutDependencies in modulesWithDependenciesCount.Where(module => module.Value == 0))
        {
            queue.Enqueue(moduleWithoutDependencies.Key);
        }

        while (queue.Count > 0)
        {
            var moduleType = queue.Dequeue();
            result.Add(moduleType);

            if (!modulesWithDependentModules.ContainsKey(moduleType))
            {
                continue;
            }

            foreach (var dependentModule in modulesWithDependentModules[moduleType])
            {
                if (modulesWithDependenciesCount[dependentModule] > 0)
                {
                    modulesWithDependenciesCount[dependentModule]--;

                    if (modulesWithDependenciesCount[dependentModule] == 0)
                    {
                        queue.Enqueue(dependentModule);
                    }
                }
            }
        }

        return result;
    }
}