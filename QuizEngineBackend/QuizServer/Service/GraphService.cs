using Neo4j.Driver.V1;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Service
{
    public class GraphService : IGraphService
    {
        IDriver driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "root"));
        public bool ConceptExists(Concept node)
        {
            throw new NotImplementedException();
        }
        public bool QuestionIdExists(QuestionIdNode node)
        {
            throw new NotImplementedException();
        }

        public IStatementResult CreateConceptNode(Concept node)
        {
            IStatementResult result;
            using (Neo4j.Driver.V1.ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
               
                result = session.Run("CREATE (n:Source {name:\"" + node.ConceptName + "\"}, {name:\"" + node.domain + "\"}) return n");
            }
            //driver.Dispose();
            return result;
           
        }
   
        public IStatementResult CreateQuestionIdNode(QuestionIdNode target)
        {
            IStatementResult result;
            using (Neo4j.Driver.V1.ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");

                result = session.Run("CREATE (n:Target {name:\"" + target.QuestionId + "\"}) return n");
            }
            //driver.Dispose();
            return result;
        }
        public IStatementResult CreateConceptwithAssociatedConcepts (ConceptMap concept)
        {
            IStatementResult result;
            using (Neo4j.Driver.V1.ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                var sourceConcept = concept.Source;
                var targetConcept = concept.Target;
                var predicate = concept.Predicate;
                result = session.Run("Match (n:Concept {name:\"" + sourceConcept +"\"}) create (m:Concept {name:\"" + targetConcept +"\"}) CREATE (n)-[x:"+ predicate +"]->(m) return n,m,x");
            }
            //driver.Dispose();
            return result;
        }
        public IStatementResult CreateConceptwithAssociatedQuestions(Triplet concept)
        {
            IStatementResult result;
            using (Neo4j.Driver.V1.ISession session = driver.Session())
            {
                //result = session.Run("create (n:Concept {Name:" + concept1.ConceptName + "}); create (n:Concept {Name:" + concept2.ConceptName + "}); match (n:Concept {Name:" + concept1.ConceptName + "}),(m:Concept) {Name:" + concept2.ConceptName + "}); create (n)-[trialrelation:" + relationship.RelationshipName + "]->(m); return n,m,trialrelation;");
                var sourceConcept = concept.source;
                var targetConcept = concept.target;
                var predicate = concept.relationship;
                result = session.Run("Match (n:Concept {name:\"" + sourceConcept + "\"}) create (m:Target {name:\"" + targetConcept + "\"}) CREATE (n)-[x:" + predicate + "]->(m) return n,m,x");
            }
            //driver.Dispose();
            return result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IStatementResult GetConceptwithRelationships(string nodename)
        {
            throw new NotImplementedException();
        }

        public IStatementResult GetGraph()
        {
            throw new NotImplementedException();
        }
    }

    public interface IGraphService
    {
        IStatementResult GetGraph();
        IStatementResult CreateConceptNode(Concept node);
        IStatementResult CreateQuestionIdNode(QuestionIdNode node);
        bool ConceptExists(Concept node);
        bool QuestionIdExists(QuestionIdNode node);
        IStatementResult GetConceptwithRelationships(string nodename);
        IStatementResult CreateConceptwithAssociatedConcepts(ConceptMap concepts);
        void Dispose();
    }
}
