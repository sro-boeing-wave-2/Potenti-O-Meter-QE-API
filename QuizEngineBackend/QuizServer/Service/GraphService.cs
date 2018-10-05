
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
        IDriver driver;

        public GraphService()
        {
            try
            {           
                driver = GraphDatabase.Driver(new Uri("bolt://neo4j"), AuthTokens.Basic("neo4j", "password"));
                Console.WriteLine("Graph Driver Initialized");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

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
                result = session.Run("merge (n:Version {name:\"" + version + "\"}) merge (m:Domain {name:\"" + domain + "\"})  merge (m)-[x:" + predicate + "]->(n) return n,m");
                //result1 = session.Run("merge (m:Domain {name:\"" + domain + "\"}) return n");
                //result = session.Run("Match (n:Version {name:\"" + version + "\"}) match (m:Domain {name:\"" + domain + "\"}) create (m)-[x:" + predicate + "]->(n) return n,m,x");
               // Console.WriteLine(" Domain Node Created " + JsonConvert.SerializeObject(result));
            }
            //Console.WriteLine("LIST OF COUNT " + list.Count());

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
                    result = session.Run("Match (n:Concept {name:\"" + sourceConcept.name + "\"}) match (m:Question {name:\"" + targetConcept.questionId + "\"}) merge (n)<-[x:" + predicate.name + "]-(m) return n,m,x");
                    //result = session.Run("Match(n:Concept) Match(m:QuestionIdNode) where(n.ConceptName = 'checmistry' AND m.QuestionId = '5db1b4f3d5c1a8cda768a') create ((n)-[x:" + predicate.name + " ]->(m)) return x");

                    //Console.WriteLine(" Relation Node Created " + JsonConvert.SerializeObject(result));
                    //Console.WriteLine("==============================================");
                }


            }
          
            return result;

        }

        public IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node, string domain)
        {
            List<ConceptMap> list = node;
            IStatementResult result = null;
            //Console.WriteLine("Length is " + JsonConvert.SerializeObject(node));
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
                    //Console.WriteLine("THIS IS CONCEPT TO CONCEPT MAPPING " + list[i].source.name);
                }

            }
            return result;

        }
        public IStatementResult CreateContentRecommendationMapping(List<ContentConceptTriplet> node, string domain)
        {
            List<ContentConceptTriplet> list = node;
           
            IStatementResult result=null;
            IStatementResult result1 = null;
            IStatementResult result2= null;
            for (int i = 0; i < list.Count; i++)
            {
                using (ISession session = driver.Session())
                {
                    var FromConcept = list[i].Target.name;
                    var url = list[i].Source.Url;
                    var title = list[i].Source.Title;
                    var predicate = list[i].Relationship.Name;
                    var tag = list[i].Source.Tags[0];
                    var taxonomy = list[i].Relationship.Taxonomy;
                    //sConsole.WriteLine("This is from concept " + taxonomy + tag + FromConcept);
                    var query = $"MERGE (n:Concept {{ name:'{FromConcept}' }}) return n";
                    Console.WriteLine(query);
                    //result = session.Run(query);

                    var conceptContentCreatorQuery = $@"
                    MERGE (n:Concept {{name: '{FromConcept}'}})
                    MERGE(m: Content {{title:'{list[i].Source.Title}',url: '{list[i].Source.Url}', tags: [{ string.Join(',', list[i].Source.Tags.Select(x => $"'{x}'"))}]}}) 
                    MERGE(n)<-[x:{list[i].Relationship.Name} {{Taxonomy: '{list[i].Relationship.Taxonomy}'}}]-(m) return n, m, x";

                    Console.WriteLine(conceptContentCreatorQuery);

                    result1 = session.Run(conceptContentCreatorQuery);
                    //result = session.Run("merge (n:Content {URL:\"" + url+ "\" ,Tags:\"" + tag + "\" })  return n");
                    //result = session.Run("match (n:Concept {name:\"" + FromConcept + "\"}) match(m:Content {URL:\"" + url + "\"  ,Tags:\"" + tag + "\"}) create (n)-[x:\"" + predicate + "\"]->(m) return n,m,x");

                    Console.WriteLine("THIS IS CONCEPT TO CONCEPT MAPPING " + JsonConvert.SerializeObject(result1));
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
                   // Console.WriteLine("This is from the graph daata" + result.ToList());
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
            IStatementResult resultRepeated;
            using (ISession session = driver.Session())
            {
                List<string> listOfQuestionId = new List<string>();
                result = session.Run("match (c:Concept)  WHERE NOT (c)<-[]-(:User{name:\"" + UserId + "\"})  and (c)-[:Concept_Of]->(:Domain{name:\"" + DomainName + "\"}) WITH  COLLECT (DISTINCT c) as ccoll Match (q:Question)-[]->(cprime:Concept) WHERE  cprime in ccoll return q LIMIT 10");
                var res = result.ToList();
              
                if (res.Count() == 10 )
                {
                    for (int i = 0; i < res.ToList().Count(); i++)
                    {
                        // Console.WriteLine("I'm INSIDE CORRECT QUESTION AREA " + JsonConvert.SerializeObject(res[i]));
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
                }
               else
                {

                    resultRepeated = session.Run("match (c:Concept)-[x]-(ul:User{name:\"" + UserId + "\"}) where (c)-[:Concept_Of]-(:Domain{name:\"" + DomainName + "\"}) WITH COLLECT (DISTINCT c) as ccoll Match (q:Question)-[r]->(cprime:Concept)<-[rel]-(u: User{ name: \"" + UserId + "\"}) WHERE cprime in ccoll  return q order by rel.Intensity limit 10");
                    Console.WriteLine("THIS IS THE RESULT " + JsonConvert.SerializeObject(result));
                    var ress = resultRepeated.ToList();
                    Console.WriteLine("THIS IS THE COUNT " + resultRepeated.ToList().Count());
                    for (int i = 0; i < ress.ToList().Count(); i++)
                    {
                        Console.WriteLine("I'm INSIDE QUESTION ARER " + JsonConvert.SerializeObject(ress[i]));
                        Object o = ress[i];
                        JObject ParsedQuestion = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object q = ParsedQuestion.GetValue("Values");
                        JObject P = JObject.Parse(JsonConvert.SerializeObject(q));
                        Object values = P.GetValue("q");
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(values));
                        Object property = prop.GetValue("Properties");
                        JObject qid = JObject.Parse(JsonConvert.SerializeObject(property));
                        string questionId = qid.GetValue("name").ToString();
                        Console.WriteLine("THIS IS THE INTENSITY QUESTION AREA " + questionId);
                        listOfQuestionId.Add(questionId);
                    }
                }

                return listOfQuestionId;

            }
        }
        public void UpdateUserConceptRelation(UserInfo userInfo, int userId)
        {
            IStatementResult result;

            List<Object> questionAttemted = userInfo.QuestionsAttempted;
            //Console.WriteLine("QUESTIONS ATTEMPTED" + JsonConvert.SerializeObject(userInfo.QuestionsAttempted));
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
                    //Console.WriteLine("This is ===============================" + res);
                    string taxonomy = Parseddetail.GetValue("taxonomy").ToString();
                    if (res)
                    {
                        result = session.Run("merge (n:Concept {name:\"" + target + "\"}) merge (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + "{Taxonomy:\"" + taxonomy + "\"} ]-> (n) on create set x.Intensity = 1  on match set x.Intensity= x.Intensity + 1 return n,m,x");
                    }
                    else
                    {
                        result = session.Run("merge(n:Concept {name:\"" + target + "\"}) merge (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + " {Taxonomy:\"" + taxonomy + "\"} ]-> (n) on create set x.Intensity = 0 return n,m,x");
                    }
                    //result = session.Run("match (n:Concept {name:\"" + target + "\"}) match (m:User { name:\"" + userId + "\" }) merge (m)-[x:" + taxonomy + "]-> (n) return n,m,x");

                }

            }

        }
        public List<ContentRecommender> GetContentRecommendations(int userId, string domain)
        {
            IStatementResult result;
            IStatementResult result1;
            List<ContentRecommender> cr = new List<ContentRecommender>();
            Dictionary<string, List<string>> ContentRecommendations = new Dictionary<string, List<string>>();
            Console.WriteLine("INSIDE GET CONTENT");
            using (ISession session = driver.Session())
            {
                result1 = session.Run("match (c:Concept)-[:Concept_Of]->(:Domain{name:\"" + domain + "\"}) WITH COLLECT (DISTINCT c) as ccoll  Match(co:Content)-[r] -> (cprime: Concept) where cprime in ccoll return co,cprime ");
                var re = result1.ToList();
                Console.WriteLine("These are CONTENTS ================" + JsonConvert.SerializeObject(re));
                if (re.Count() != 0)
                {
                    Console.WriteLine("COUNT ==============" + re.Count());
                    for (int i = 0; i < re.ToList().Count(); i++)
                    {
                        ContentRecommender c = new ContentRecommender();
                        Object o = re[i];
                        JObject ParsedQuestion = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object q = ParsedQuestion.GetValue("Values");
                        JObject P = JObject.Parse(JsonConvert.SerializeObject(q));
                        Object values = P.GetValue("co");                       
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(values));
                        Object property = prop.GetValue("Properties");
                       
                        JObject qid = JObject.Parse(JsonConvert.SerializeObject(property));
                        string url = qid.GetValue("url").ToString();
                        string title = qid.GetValue("title").ToString();
                        JToken tags = qid.GetValue("tags");
                        //string id = prop.GetValue("Id").ToString();
                        Object Concept = P.GetValue("cprime");
                        JObject ConceptProp = JObject.Parse(JsonConvert.SerializeObject(Concept));
                        Object Conceptproperty = ConceptProp.GetValue("Properties");
                        JObject Cname = JObject.Parse(JsonConvert.SerializeObject(Conceptproperty));
                        string name = Cname.GetValue("name").ToString();
                        //Console.WriteLine("THIS IS NAmE OF CONCEPT =====" + name);
                        //c.Id = id;
                        c.conceptName = name;
                        c.url = url;
                        c.title = title;
                        c.tags = tags.ToObject<List<string>>();
                        cr.Add(c);
                        var tag = tags.ToList();
                       
                        
                    }
                }
                result = session.Run("match (c:Concept)<-[x]-(ul:User{name:\"" + userId + "\"}) where (c)-[:Concept_Of]->(:Domain{name:\"" + domain + "\"}) WITH COLLECT (DISTINCT c) as ccoll Match(co:Content)-[r]->(cprime: Concept)<-[rel]-(u: User{ name:\"" + userId + "\"}) WHERE cprime in ccoll and  rel.Intensity<2 and rel.Taxonomy=r.Taxonomy return co,cprime ");
                var res = result.ToList();
                Console.WriteLine("COUNT " + result.ToList().Count());
                if (result.ToList().Count() != 0)
                {
                    for (int i = 0; i < res.ToList().Count(); i++)
                    {
                        ContentRecommender c = new ContentRecommender();
                        Object o = res[i];
                        JObject ParsedQuestion = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object q = ParsedQuestion.GetValue("Values");
                        JObject P = JObject.Parse(JsonConvert.SerializeObject(q));
                        Object values = P.GetValue("co");
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(values));
                        Object property = prop.GetValue("Properties");
                        
                        JObject qid = JObject.Parse(JsonConvert.SerializeObject(property));
                        string url = qid.GetValue("url").ToString();
                        string title = qid.GetValue("title").ToString();
                        JToken tags = qid.GetValue("tags");
                        
                       
                        //string id = prop.GetValue("Id").ToString();
                        Object Concept = P.GetValue("cprime");
                        JObject ConceptProp = JObject.Parse(JsonConvert.SerializeObject(Concept));
                        Object Conceptproperty = ConceptProp.GetValue("Properties");
                        JObject Cname = JObject.Parse(JsonConvert.SerializeObject(Conceptproperty));
                        string name = Cname.GetValue("name").ToString();
                        //Console.WriteLine("THIS IS NAmE OF CONCEPT =====" + name);
                        //c.Id = id;
                        c.conceptName = name;
                        //c.conceptName = name;
                        c.url = url;
                        c.title = title;
                        c.tags = tags.ToObject<List<string>>();
                        cr.Add(c);
                        var tag = tags.ToList();
                        
                    }
                }
                Console.WriteLine("THIS IS THE CONTENT " + JsonConvert.SerializeObject(cr));

                return cr;
            }
            
        }

        public List<BestDomain> GetBestDomain(int userID)
        {
            IStatementResult result;
            List<BestDomain> DomainList = new List<BestDomain>();
           
           
            using (ISession session = driver.Session())
            {
                result = session.Run("MATCH (u:User{name:\"" + userID + "\"})-[rel]-(c:Concept)-[:Concept_Of]-(n:Domain) return {intensity: sum(rel.Intensity), domain: n.name} AS domains");

                var re = result.ToList();
                
                if (re.Count() != 0)
                {
                    Console.WriteLine("COUNT ==============" + re.Count());
                    for (int i = 0; i < re.ToList().Count(); i++)
                    {
                        BestDomain bd = new BestDomain();
                        Object o = re[i];
                        JObject ParsedDomain = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object ParsedDomainValue = ParsedDomain.GetValue("Values");
                        JObject DomainValuesJObject = JObject.Parse(JsonConvert.SerializeObject(ParsedDomainValue));
                        Object domains = DomainValuesJObject.GetValue("domains");
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(domains));
                        int intensityObj = (int)prop.GetValue("intensity");
                        string domain = prop.GetValue("domain").ToString();
                        bd.Domain = domain;
                        bd.Intensity = intensityObj;
                        DomainList.Add(bd);
                    }
                }
                return DomainList;
            }
        }

        public List<BestConceptOfDomain> GetBestConceptOfDomain(int userID, string domain)
        {
            IStatementResult result;
            List<BestConceptOfDomain> DomainList = new List<BestConceptOfDomain>();


            using (ISession session = driver.Session())
            {
                result = session.Run("MATCH (u:User{name:\"" + userID +"\"})-[rel]-(c:Concept)-[:Concept_Of]-(n:Domain{name:\""+domain+"\"}) return {intensity: sum(rel.Intensity), Concept:c.name} AS concepts");

                var re = result.ToList();

                if (re.Count() != 0)
                {
                    Console.WriteLine("COUNT ==============" + re.Count());
                    for (int i = 0; i < re.ToList().Count(); i++)
                    {
                        BestConceptOfDomain bd = new BestConceptOfDomain();
                        Object o = re[i];
                        JObject ParsedDomain = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object ParsedDomainValue = ParsedDomain.GetValue("Values");
                        JObject DomainValuesJObject = JObject.Parse(JsonConvert.SerializeObject(ParsedDomainValue));
                        Object domains = DomainValuesJObject.GetValue("concepts");
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(domains));
                        int intensityObj = (int)prop.GetValue("intensity");
                        string concept = prop.GetValue("Concept").ToString();
                        bd.Concept = concept;
                        bd.Intensity = intensityObj;
                        DomainList.Add(bd);
                    }
                }
                return DomainList;
            }
        }


        public List<BestTaxonomy> GetBestTaxonomy(int userID)
        {
            IStatementResult result;
            List<BestTaxonomy> DomainList = new List<BestTaxonomy>();


            using (ISession session = driver.Session())
            {
                result = session.Run("MATCH (u:User {name: \""+ userID+ "\"})-[x]->(c:Concept) return {intensity: max(x.Intensity), Taxonomy:x.Taxonomy} AS taxonomy");

                var re = result.ToList();

                if (re.Count() != 0)
                {
                    Console.WriteLine("COUNT ==============" + re.Count());
                    for (int i = 0; i < re.ToList().Count(); i++)
                    {
                        BestTaxonomy bd = new BestTaxonomy();
                        Object o = re[i];
                        JObject ParsedDomain = JObject.Parse(JsonConvert.SerializeObject(o));
                        Object ParsedDomainValue = ParsedDomain.GetValue("Values");
                        JObject DomainValuesJObject = JObject.Parse(JsonConvert.SerializeObject(ParsedDomainValue));
                        Object domains = DomainValuesJObject.GetValue("taxonomy");
                        JObject prop = JObject.Parse(JsonConvert.SerializeObject(domains));
                        int intensityObj = (int)prop.GetValue("intensity");
                        string taxonomy = prop.GetValue("Taxonomy").ToString();
                        bd.Taxonomy = taxonomy;
                        bd.Intensity = intensityObj;
                        DomainList.Add(bd);
                    }
                }
                return DomainList;
            }
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }



    }
}
