# News Article Reader

A command line tool to display news articles from RSS and ATOM feeds. By default the tools will display the latest 5 articles from the BBC news feed.
The user can ask the tool to display latest N stories from known feeds or directly from RSS/ATOM URIs.
The user can also ask the tool to search for articles whose TITLE contains a search term, or display articles that exclude the search term.

Solution compiled in Visual Studio 2013 Community Edition

Usage:

    NewsArticles [/with[out] searchTerm] [/latest count] [feedName]

    NewsArticles [/with[out] searchTerm] [/latest count] [/rss|/atom feedUri]

      /with     "string"     display news articles containing "string"

      /without  "string"     display news articles not containing "string"

      /latest   N            display latest N articles if no search criteria specified

                             display at most N latest articles satisfying search criteria

      /rss      "string"     The URI of the RSS news feed (mutually exclusive with /atom)

      /atom     "string"     The URI of the ATOM news feed (mutually exclusive with /rss)

      feedName               Recognized Feed names (Defaults to BBC RSS feed):
                             BBC                (RSS)   British Broadcasting Network

                             CNN                (RSS)   Cable News Network

                             ABC-INT            (RSS)   ABC International

                             ABC-MOST-READ      (RSS)   ABC Most read stories

                             USGS-ALL-QUAKES    (ATOM)  USGS All earthquakes in past hour

Design

          MODEL                CONTROLLER                   VIEW

          IStoryContent
		      /\
			  \/
			  |
		  IStories <>-------------Printer-----------------<>ConsoleOutput
		                          |    |                          |
								 /\   /\                         /\
								 \/   \/                         \/
               DisplayStoryFactory     StoriesFactory         RssDisplayStory
			                                                  AtomDisplayStory
															      |
																 /\
																 \/
														      IDisplayItem