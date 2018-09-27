
using Neo4j.Driver.V1;
using Newtonsoft.Json;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace QuizServer.Service
{
    public class GraphService : IGraphService
    {
        readonly string ConsulIP = Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
        IDriver driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
        public bool ConceptExists(Concept node)
        {
            throw new NotImplementedException();
        }
        public bool QuestionIdExists(QuestionIdNode node)
        {
            throw new NotImplementedException();
        }
       
        public IStatementResult CreateConceptToQuestionMapping(List<Triplet> node, string version , string domain)
        {
            List<Triplet> list = node;
            IStatementResult result = null;
            using (ISession session = driver.Session())
            {
               
                var predicate = "Of";
                result = session.Run("CREATE (n:Version {name:\"" + version + "\"}) return n");
                result = session.Run("CREATE (n:Domain {name:\"" + domain + "\"}) return n");
                result = session.Run("Match (n:Version {name:\"" + version + "\"}) match (m:Domain {name:\"" + domain + "\"}) CREATE (m)-[x:" + predicate + "]->(n) return n,m,x");
                Console.WriteLine("version node created " + JsonConvert.SerializeObject(result));
                Console.WriteLine("=================================");
            }
            Console.WriteLine("LIST OF COUNT " + list.Count());

            for (int i = 0; i < list.Count; i++)
            {
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                    result = session.Run("CREATE (n:Concept {name:\"" + list[i].source.name + "\"}) return n");
                    Console.WriteLine("Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                    result = session.Run("CREATE (n:QuestionIdNode {name:\"" + list[i].target.questionId + "\"}) return n");
                    Console.WriteLine(" Question Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                    var sourceConcept = list[i].source;
                    var targetConcept = list[i].target;
                    var predicate = list[i].relationship;
                    result = session.Run("Match (n:Concept {name:\"" + sourceConcept.name + "\"}) match (m:QuestionIdNode {name:\"" + targetConcept.questionId + "\"}) CREATE (m)-[x:" + predicate.name + "]->(n) return n,m,x");
                    //result = session.Run("Match(n:Concept) Match(m:QuestionIdNode) where(n.ConceptName = 'checmistry' AND m.QuestionId = '5db1b4f3d5c1a8cda768a') create ((n)-[x:" + predicate.name + " ]->(m)) return x");

                    Console.WriteLine(" Relation Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }


            }
            using (ISession session = driver.Session())
            {
                var predicate = "Has";

                result = session.Run("match (n:Concept) match(m:Domain) CREATE (m)-[x:" + predicate + "]->(n) return n,m,x");

            }
            return result;

        }

        public IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node)
        {
            List<ConceptMap> list = node;
            IStatementResult result = null;

            for (int i = 0; i < list.Count; i++)
            {
                using (ISession session = driver.Session())
                {
                    var FromConcept = list[i].source.name;
                    var Toconcept = list[i].target.name;
                    var predicate = list[i].relationship.name;

                    result = session.Run("match (n:Concept {name:\"" + FromConcept + "\"}) match (m:Concept { name:\"" + Toconcept + "\" }) create(m)-[x:" + predicate + "]-> (n) return n,m,x");
                  
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
            using(ISession session = driver.Session())
            {
                result = session.Run("create (n:User{name:\"" + userID + "\"}) return n");
            }
        }

        public IStatementResult GetQuestionsFromGraph(int UserId, string DomainName)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("match (n:Domain{name:\"" + DomainName + "\"}) return n");
                Console.WriteLine("THIS IS THE RESULT" + result.Single()[0].As<string>());
                Console.WriteLine("Rsult from the graph " + JsonConvert.SerializeObject(result));
                //result.Single()[0].As<string>();
                //foreach (var r in result)
                //{
                //    //Get as an INode instance to access properties.
                //    var node = r["science"].As<INode>();
                //    Console.WriteLine("THIS IS THE NODE " + node.Labels);
                //    //Properties are a Dictionary<string,object>, so you need to 'As' them
                //    var age = node["age"].As<int>();
                //    var name = node["name"].As<string>();

                //    Console.WriteLine($"{name} is {age} years old.");
                //}
                return result;

            }
        }
        public void UpdateUserConceptRelationForWrongQuestion(string questionId, int userId)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("MATCH (a:QuestionIdNode{name: \"" + questionId + "\"}) match(b:Concept) match (c:User{name:\"" + userId + "\"}) CREATE(b) -[r: weak]->(c) RETURN b,c");

            }
        }
        public void UpdateUserConceptRelation(string questionId, int userId)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                result = session.Run("MATCH (a:QuestionIdNode{name: \"" + questionId + "\"}) match(b:Concept) match (c:User{name:\"" + userId + "\"}) CREATE(b) -[r: strong]->(c) RETURN b,c");

            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

      
    
    }
}
