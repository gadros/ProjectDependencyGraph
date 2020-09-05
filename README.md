# ProjectDependencyGraph
a Windows console application.<br>
**problem:** you're embarking on some project. the project's work can be described through it's steps (.do 'this', then do 'that'), through efforts estimate and there's a relationship between these steps (can build the first floor only after having the foundations). all these are described in an Excel worksheet. but it's hard to visualize this and see easily set integration points or calculate the critical path.<br>
**solution:** given an Excel file that describes work in some project the application generates a graph that describes the work found in the Excel. the graph is written to a .pdf document.<br><br>
It uses GraphViz and relies on EpPlus (nuget) to do it's work.<br><br>
In order for it to function you need to describe the different values such as  the relevant Excel's columns in the App.config usersSettings element.<br>
required configuration values:<br>
**WorkBreakDownSheet** - the name of the data sheet inside the Excel file that contains the breakdown of the work<br>
**RowKeyColumn** - the column number (1 for the 1st column, 2 for the 2nd column...)<br>
**FirstDataRow** - the row number from which work items are described (maybe you're using headers? etc.)<br>
**ParentRowsColumn** - the column can contains comma delimited row numbers where each represents work that precedes this work. example: if the column in row 3 contains 10,2 it means that the wrok in row 3 comes after work in row 2 and row 10 - it's dependent on them. an empty value means the work is independent.<br>
**EstimatedffortColumn** - the estimate of work effort to execute this task (no assumption or understanding of the units you're using)<br>
**DescriptionColumnFailback** - the column that describes the work (set to the node). it accepts comma delimited column numbers. so that if no description is given in the column then it will try the next. example: setting 5, 4, 3 means it will attempt to find a description in the 5th column and if not found will try the 4th and 3rd<br>