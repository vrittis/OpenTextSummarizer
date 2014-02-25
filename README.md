OpenTextSummarizer
==================

.net port and adaptation of libots, initially ported by PatrickBurrows

This version of OpenTextSummarizer has been pushed to Github in order to correct some bugs and add some behaviors that i find interesting.
The initial port this version started from is located on codeplex (http://ots.codeplex.com) and was written for .Net 2. This version will target .Net 4.0 and may use some functionalities that will prevent you from using it under .Net 2.0

Roadmap:
* --Add qualification tests-- (almost done, i will stop now for the time and starting changing behaviors)
* fix bugs (stemmer bug on lowercasing of some words only)
* add configurable behavior: sentence scoring mainly will benefit from these changes
