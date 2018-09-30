
using Neo4j.Driver.V1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Potentiometer.Core.QuestionTypes;
namespace QuizServer.Service
{
    public class GraphService : IGraphService
    {
        readonly string ConsulIP = Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
        IDriver driver = GraphDatabase.Driver("bolt://neo4j", AuthTokens.Basic("neo4j", "password"));
        public bool ConceptExists(Concept node)
        {
            throw new NotImplementedException();
        }
        public bool QuestionIdExists(QuestionIdNode node)
        {
            throw new NotImplementedException();
        }

        public IStatementResult CreateConceptToQuestionMapping(List<Triplet> node, string version, string domain)
        {
            List<Triplet> list = node;
            IStatementResult result = null;
            using (ISession session = driver.Session())
            {

                var predicate = "Of";
                result = session.Run("create (n:Version {name:\"" + version + "\"}) create (m:Domain {name:\"" + domain + "\"})  merge (m)-[x:" + predicate + "]->(n) return n,m");
                //result = session.Run("create (n:Domain {name:\"" + domain + "\"}) return n");
                //result = session.Run("Match (n:Version {name:\"" + version + "\"}) match (m:Domain {name:\"" + domain + "\"}) create (m)-[x:" + predicate + "]->(n) return n,m,x");
               
            }
            Console.WriteLine("LIST OF COUNT " + list.Count());

            for (int i = 0; i < list.Count; i++)
            {
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                    var predicate = "Concept_Of";
                    result = session.Run("merge (n:Concept {name:\"" + list[i].source.name + "\"}) return n"); 
                    result = session.Run("match (n:Concept {name:\"" + list[i].source.name + "\"}) match(m:Domain {name:\"" + domain + "\"}) merge (n)-[x:" + predicate + "]->(m) return n,m,x");
                   
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                    result = session.Run("merge (n:Question {name:\"" + list[i].target.questionId + "\"}) return n");
                   
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                    var sourceConcept = list[i].source;
                    var targetConcept = list[i].target;
                    var predicate = list[i].relationship;
                    result = session.Run("Match (n:Concept {name:\"" + sourceConcept.name + "\"}) match (m:Question {name:\"" + targetConcept.questionId + "\"}) merge (n)-[x:" + predicate.name + "]->(m) return n,m,x");
                    //result = session.Run("Match(n:Concept) Match(m:QuestionIdNode) where(n.ConceptName = 'checmistry' AND m.QuestionId = '5db1b4f3d5c1a8cda768a') create ((n)-[x:" + predicate.name + " ]->(m)) return x");

                    Console.WriteLine(" Relation Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }


            }
          
            return result;

        }

        public IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node, string domain)
        {
            List<ConceptMap> list = node;
            IStatementResult result = null;
            Console.WriteLine("Length is " + JsonConvert.SerializeObject(node));
            for (int i = 0; i < list.Count; i++)
            {
                using (ISession session = driver.Session())
                {
                    var FromConcept = list[i].source.name;
                    var Toconcept = list[i].target.name;
                    var predicate = list[i].relationship.name;
                    result = session.Run("merge (n:Concept {name:\"" + FromConcept + "\"}) return n");
                    result = session.Run("merge (n:Concept {name:\"" + Toconcept + "\"}) return n");
                    result = session.Run("match (n:Concept {name:\"" + FromConcept + "\"}) match(m:Domain {name:\"" + domain + "\"}) merge (n)-[x:Concept_Of]->(m) return n,m,x");
                    result = session.Run("match (n:Concept {name:\"" + Toconcept + "\"}) match(m:Domain {name:\"" + domain + "\"}) merge (n)-[x:Concept_Of]->(m) return n,m,x");

                    result = session.Run("match (n:Concept {name:\"" + FromConcept + "\"}) match (m:Concept { name:\"" + Toconcept + "\" }) merge (m)-[x:" + predicate + "]-> (n) return n,m,x");
                    Console.WriteLine("THIS IS CONCEPT TO CONCEPT MAPPING " + list[i].source.name);
                }

            }
            return result;

        }
        public bool IsDomainExist(string domain)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("match (n:Domain{name:\"" + domain + "\"}) return n");

                if (result.ToList().Count == 0)
                {
                    Console.WriteLine("This is from the graph daata" + result.ToList());
                    return false;
                }

                return true;

            }
        }
        public bool IsUserExist(int userId)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("match (n:User{name:\"" + userId + "\"}) return n");

                if (result.ToList().Count == 0)
                {

                    return false;
                }

                return true;

            }
        }
        public void CreateUser(int userID)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("merge (n:User{name:\"" + userID + "\"}) return n");
            }
        }

        public List<string> GetQuestionsFromGraph(int UserId, string DomainName)
        {
            IStatementResult result;

            using (ISession session = driver.Session())
            {
                List<string> listOfQuestionId = new List<string>();
                result = session.Run("match (c:Concept)  WHERE NOT (c)<-[]-(:User{name:\"" + UserId + "\"})  and (c)-[:Concept_Of]->(:Domain{name:\"" + DomainName + "\"}) WITH  COLLECT (DISTINCT c) as ccoll Match (q:Question) <-[]-(cprime:Concept) WHERE  cprime in ccoll return q LIMIT 6");
                var res = result.ToList();
                for (int i = 0; i < res.ToList().Count(); i++)
                {
                    Object o = res[i];
                    JObject ParsedQuestion = JObject.Parse(JsonConvert.SerializeObject(o));
                    Object q = ParsedQuestion.GetValue("Values");
                    JObject P = JObject.Parse(JsonConvert.SerializeObject(q));
                    Object values = P.GetValue("q");
                    JObject prop = JObject.Parse(JsonConvert.SerializeObject(values));
                    Object property = prop.GetValue("Properties");
                    JObject qid = JObject.Parse(JsonConvert.SerializeObject(property));
                    string questionId = qid.GetValue("name").ToString();

                    listOfQuestionId.Add(questionId);
                }


                return listOfQuestionId;

            }
        }
        public void UpdateUserConceptRelation(UserInfo userInfo, int userId)
        {
            IStatementResult result;

            List<Object> questionAttemted = userInfo.QuestionsAttempted;
            Console.WriteLine("QUESTIONS ATTEMPTED" + JsonConvert.SerializeObject(userInfo.QuestionsAttempted));
                using (ISession session = driver.Session())
                {


                for (int i = 0; i < questionAttemted.Count; i++)
                {
                    
                    Object o = questionAttemted[i];
                    JObject Parseddetail = JObject.Parse(JsonConvert.SerializeObject(o));
                    string questionType = Parseddetail.GetValue("questionType").ToString();
                    System.Reflection.Assembly b = System.Reflection.Assembly.Load("Potentiometer.Core");
                    Type QuestionType = b.GetType("Potentiometer.Core.QuestionTypes." + questionType);
                    object instanceObjectOfQuestion = Activator.CreateInstance(QuestionType);
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(o), instanceObjectOfQuestion);
                    bool res = (bool)QuestionType.InvokeMember("Evaluate", BindingFlags.InvokeMethod, null, instanceObjectOfQuestion, new object[0]);
                    string questionId = Parseddetail.GetValue("questionId").ToString();
                    string concept = Parseddetail.GetValue("conceptTags").ToString();
                    JArray concepts = JArray.Parse(concept);
                    JToken target = concepts[0];
                    Console.WriteLine("This is ===============================" + res);
                    string taxonomy = Parseddetail.GetValue("taxonomy").ToString();
                    if (res)
                    {
                        result = session.Run("match (n:Concept {name:\"" + target + "\"}) match (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + " ]-> (n) on create set x.Intensity = 1 on match set x.Intensity= x.Intensity + 1 return n,m,x");
                    }
                    else
                    {
                        result = session.Run("match (n:Concept {name:\"" + target + "\"}) match (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + "]-> (n) on create set x.Intensity = 0 on match set x.Intensity= x.Intensity - 1 return n,m,x");
                    }
                    //result = session.Run("match (n:Concept {name:\"" + target + "\"}) match (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + "]-> (n) return n,m,x");

                }

            }

        }
       
        public void Dispose()
        {
            throw new NotImplementedException();
        }



    }
}
