public class WordCount
{
    // Start is called before the first frame update
    static public float GetWordCount(string text)
    {
        int wordCount = 0, index = 0;
        // skip whitespace until first word
        while (index < text.Length && char.IsWhiteSpace(text[index]))
            index++;

        while (index < text.Length)
        {
            // check if current char is part of a word
            while (index < text.Length && !char.IsWhiteSpace(text[index]))
                index++;

            wordCount++;

            // skip whitespace until next word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;
        }

        
        return wordCount;
    }


}
