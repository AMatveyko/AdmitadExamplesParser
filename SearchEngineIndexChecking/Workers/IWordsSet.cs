namespace SearchEngineIndexChecking.Workers
{
    internal interface IWordsSet
    {
        string GetPhrase(int length = 1);
        string GetPhraseRandomLength();
    }
}