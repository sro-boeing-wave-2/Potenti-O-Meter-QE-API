
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
        IDriver driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
        public bool ConceptExists(Concept node)
        {
            throw new NotImplementedException();
        }
        public bool QuestionIdExists(QuestionIdNode node)
        {
            throw new NotImplementedException();
        }

        public IStatementResult CreateConceptNode(List<Triplet> node)
        {
            List<Triplet> list = node;
            IStatementResult result =null;
            for (int i=0;i<list.Count;i++)
            {
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                    
                    result = session.Run("CREATE (n:Concept {name:\"" + list[i].source.ConceptName + "\"}) return n");
                    Console.WriteLine("Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                    result = session.Run("CREATE (n:QuestionIdNode {name:\"" + list[i].target.QuestionId + "\"}) return n");
                    Console.WriteLine(" Question Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }
                using (ISession session = driver.Session())
                {
                    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                    var sourceConcept = list[i].source;
                    var targetConcept = list[i].target;
                    var predicate = list[i].relationship;
                    result = session.Run("Match (n:Concept {name:\"" + sourceConcept.ConceptName + "\"}) match (m:QuestionIdNode {name:\"" + targetConcept.QuestionId + "\"}) CREATE (n)-[x:" + predicate.name + "]->(m) return n,m,x");
                    //result = session.Run("Match(n:Concept) Match(m:QuestionIdNode) where(n.ConceptName = 'checmistry' AND m.QuestionId = '5db1b4f3d5c1a8cda768a') create ((n)-[x:" + predicate.name + " ]->(m)) return x");
                   
                    Console.WriteLine(" Relation Node Created " + JsonConvert.SerializeObject(result));
                    Console.WriteLine("==============================================");
                }

            }
            return result;
            //using (Neo4j.Driver.V1.ISession session = driver.Session())
            //{
            //    //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

            //    result = session.Run("CREATE (n:Concept {name:\"" + node.ConceptName + "\"}, {name:\"" + node.domain + "\"}) return n");
            //}
            //driver.Dispose();


        }


        public IStatementResult GetGraph()
        {
            throw new NotImplementedException();
        }



        public IStatementResult GetConceptwithRelationships(string nodename)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

          public IStatementResult CreateQuestionIdNode(QuestionIdNode target)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                result = session.Run("CREATE (n:QuestionIdNode {name:\"" + target.QuestionId + "\"}) return n");
            }
            //driver.Dispose();
            return result;
        }
        public IStatementResult CreateConceptwithAssociatedConcepts(ConceptMap concept)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                var sourceConcept = concept.Source;
                var targetConcept = concept.Target;
                var predicate = concept.Predicate;
                result = session.Run("Match (n:Concept {name:\"" + sourceConcept + "\"}) create (m:Concept {name:\"" + targetConcept + "\"}) CREATE (n)-[x:" + predicate + "]->(m) return n,m,x");
            }
            //driver.Dispose();
            return result;
        }
        public IStatementResult CreateConceptwithAssociatedQuestions(Triplet concept)
        {
            IStatementResult result;
            using (ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                var sourceConcept = concept.source;
                var targetConcept = concept.target;
                var predicate = concept.relationship;
                result = session.Run("Match (n:Concept {name:\"" + sourceConcept + "\"}) create (m:QuestionIdNode {name:\"" + targetConcept + "\"}) CREATE (n)-[x:" + predicate + "]->(m) return n,m,x");
            }
            //driver.Dispose();
            return result;
        }
    }
}
