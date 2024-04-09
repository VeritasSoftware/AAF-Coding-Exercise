## Technical questions**

Please answer the following questions in a markdown file called `Answers to technical questions.md`.

- How long did you spend on the coding test? What would you add to your solution if you had more time? If you didn't spend much time on the coding test then use this as an opportunity to explain what you would add.
  
  I spent exactly 2 hours on the test. 

- What was the most useful feature that was added to the latest version of C#? Please include a snippet of code that shows how you've used it.
  
  string interpolation is really useful recent feature. 
  I have used it like below:
  $"client with client id \{clientId} not found."

- How would you track down a performance issue in production? Have you ever had to do this?
  
  Yes, I have done this. 
  You would look at the log files. 
  Narrow down which part of the code is slowing down the execution.
  Add logging statements where required.
  You can view these logs in Application Insights too.
  
  You can add Health Checks to your application, which could help dig deeper into the cause.
  
  If determined to be SQL related, you can use tools like Sql Profiler etc to go deeper.
