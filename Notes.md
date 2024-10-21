# Online Betting Data Capture and Analytics System

## Thoughts:

After looking at the requirements and what the post method is supposed to look like and also looking at the test data from .test.

- Use CasinoWager Model as my table which will simplify all operations that I need to do for this. Will also fit the test data very well to avoid creating related tables that will have null additional values or tables with just primary columns

  - Create DB and CasinoWagers table and use WagerId as the primary key
  - Create Non Clustered Index using the AccountId column as it is the most used field for most requirements
  - Initially thought we are going to need 2 stored procedures one for the top spenders another for player wagers

## General Solution Architecture

![](/general-architecture.drawio.svg)

## API Architecture

![](/api-architecture.drawio.svg)

## Service Architecture

![](/service-architecture.drawio.svg)

## Challenges and Thoughts

- I realised it very early on that getting the list of wagers and the page info wasn't going to play well with flat data, which meant 2 queries that threw the stored procedure approach for player wagers out the window.
- I had issues for a long time loading the entire 7000 records into the db, first issue was with the \_channel timing out and connection factory logging off - fx was to trigger a refresh and update the timeouts limits
- second was with sql exception timeout while loading the records - the fix was to add timeout value to the method and create a stored procedure for adding the records
- I also had issues with my endpoints not getting feed back on second queries, I realised that the default consumer setup is to read 1 message so I updated to read multiple message and it all worked

### All in all it is a great project that is engaging and out of the box, I am going to build on top of what I have already implemented
