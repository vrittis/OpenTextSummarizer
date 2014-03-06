OpenTextSummarizer
==================

.net port and adaptation of libots, initially ported by PatrickBurrows to C# and forked by yours truly as a kind of exercice after a review at http://samy.beaudoux.net/blog/?p=23

#Usage

##Basic

To use the original OpenTextSummarizer algorithm, just call the static `Summarize` method on the summarizer. You can pass content through a `IContentProvider` implementation.
There are two implementations matching what existed in the original library, `DirectTextContentProvider` and `FileContentProvider`.
The second argument is a `ISummarizerArguments` implementation, the default one being `SummarizerArguments`.

```csharp
var summarizedDocument = OpenTextSummarizer.Summarizer.Summarize(
                new OpenTextSummarizer.FileContentProvider("TextualData\\AutomaticSummarization.txt"),
                new SummarizerArguments() 
				{
					Language = "en",
					MaxSummarySentences = 5
				});
```

##Advanced

It is possible to change some behavior of the summarizer. Basically what is happening is that the summarizing engine will pipe three operations together in order to create the summary:
* parsing: splitting text into sentences and sentences into words. This part is in charge of a `IContentParser` implementation
* analyzing: scoring text units and sentences in order to determine the importance of each. This part is in charge of a `IContentAnalyzer` implementation
* summarizing: selecting text units and sentences that will compose the final summary. This part is in charge of a `IContentSummarizer` implementation

Each of these interfaces can be implemented in order to plug a different behavior into the summarizer. In order to swap the default implementation with yours, you can use the following properties on the `ISummarizerArguments`: `ContentParser`, `ContentAnalyzer` and `ContentSummarizer`.
Each property is a lambda that acts as a factory for types implementing this interface. Just swap the default implementation with yours in the lambda and it will be picked up during the summarizing process.

Here is an example of a `IContentParser`:

```csharp
public class TelegramContentParser : IContentParser
{
    public List<Sentence> SplitContentIntoSentences(string Content)
    {
        return Content.Split(new string[] { "STOP" }, StringSplitOptions.RemoveEmptyEntries)
            .Select((currentString, currentIndex) => new Sentence() {
				OriginalSentence = currentString,
				OriginalSentenceIndex = currentIndex
			})
            .ToList();
    }

    public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
    {
        return sentence.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(currentString => new TextUnit() {
				RawValue = currentString,
				FormattedValue = currentString.ToLower(),
				Stem = currentString.ToLower()
			})
            .ToList();
    }
}

//...

var summarizedDocument = OpenTextSummarizer.Summarizer.Summarize(
                new OpenTextSummarizer.FileContentProvider("TextualData\\AutomaticSummarization.txt"),
                new SummarizerArguments() {
					Language = "en",
					MaxSummarySentences = 5,
					ContentParser = () => new TelegramContentParser()
				});
```

Have a look at the interfaces to see what each will needs for implementation.

#Notes

This version of OpenTextSummarizer has been pushed to Github in order to correct some bugs. I've tried to make it extensible a bit more than what was initially ported.
The initial port this version started from is located on codeplex (http://ots.codeplex.com) and was written for .Net 2. It now uses .Net 3.5

Roadmap:
* ~~Add qualification tests~~ (almost done, i will stop now for the time and starting changing behaviors)
* ~~fix bugs (stemmer bug on lowercasing of some words only)~~
* add configurable behavior
