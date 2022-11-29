# TweetClientApp.TweeClient.WebApi
This is a C# .Net Core Rest API which exposes one end point to get the Twitter metrics, total tweets streamed and the top ten hashtags

This is a monolith Web API and is divided in to multi layers
1) TweetClient.WebApi -> This is the API, end point for the client to connect over http
2) TweetClient.Application -> this has the application use case logic
3) TweetClient.Application.Tests -> this has the tests for the application code 
4) TweetClient.Infrastructure -> This is the infrastructure layer, has code to connect to the twitter Sample Stream API and stream the tweets
5) TweetClient.DataAccess -> This is the Data access layer, has ORM code to connect to DB (LiteDb)

# TweetClientApp.TweeClient.Web
This is an Angular Front end application which connects to API and display the metrics
