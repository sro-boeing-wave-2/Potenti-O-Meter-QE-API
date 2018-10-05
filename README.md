

**Quiz Engine**
===

**Project source can be downloaded from git**
[Click here to go to the github repository](https://github.com/sro-boeing-wave-2/Potenti-O-Meter-QE-API)

###**Know about QuizEngine**
QuizEngine is a microservice which is used in our main product Potentiometer, which is adaptive over the quizes for a particular User. Suppose a User takes a quiz for the first time, he gets questions from certain concepts of a particular domain say C language and for the next quiz, the set of questions are decided based on his previous response. If he answered a question, next question will be from other concepts and taxanomy level or questions from the subconcepts will be given. Bloom's taxanomy has greater role to play in decided the next question and the Knowledge state of a User is saved in terms of a graph in neo4j.




###**Authors & Contributor list**
Deepika M M
Shashidhar B

All other known bugs and fixes can be sent to 
[deepikamm12@gmail.com](deepikamm12@gmail.com) or [shashishashidhar70@gmail.com](shashishashidhar70@gmail.com)

Reported bugs/fixes will be submitted to correction.

###**Main technologies used are**

 - .NET core
 - Angular 6
 - Signal R
 - Mongo DB
 - Neo4j


###**Once you have cloned the repository, follow instructions below**

 1. Open ```GitBash``` in the directory
 2. ```cd``` to ```Potenti-O-Meter-QE-API``` directory
 3. Modify ```docker-compose.yml``` for running in local machine
 4. Execute this command ```docker-compose up --build```
 5. Do ```localhost:8010/api/question/getall``` to get all the responses of all the users in Mongo DB

###**Sending feedback and pull requests**
We'd appreciate your feedback, improvements and ideas. You can create new issues at the issues section, do pull requests and/or send emails to [deepikamm12@gmail.com](deepikamm12@gmail.com) or [shashishashidhar70@gmail.com](shashishashidhar70@gmail.com)
 
 