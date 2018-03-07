namespace SolutionGenerator
{
    interface ISolutionGenerator
    {
        void GenerateSolution(string path, ISolution template, Scenario scenario);
    }
}
