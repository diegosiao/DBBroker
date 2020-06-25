//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace DBBrokerClient.Model
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class BusinessRule
//    {
//        public string Code { get; set; }

//        public string Title { get; set; }

//        public string WhatAllows { get; set; }

//        public string WhatDisallow { get; set; }

//        //public List<IBusinessEvaluationUnity> Evaluations { get; set; }

//        public bool Enable { get; set; }

//        public void Evaluate()
//        {
//            //throws an exception
//        }
//    }

//    /* 
//     * Maybe that is not a good abstraction,
//     * because it is reasonable to assume that
//     * evey application interaction must have 
//     * the potential to change persisted data.
//     * 
//     * That should be abstracted.
//     */
//    public enum BusinessRuleType
//    {
//        /// <summary>
//        /// Defines the access to some features
//        /// </summary>
//        Permission,
        
//        /// <summary>
//        /// Represents 
//        /// </summary>
//        Action
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    interface IBusinessEvaluationUnity
//    {
//        public BusinessRule BusinessRule { get; set; }

//        public string LeftExpression;

//        public BusinessEvaluationUnityType EvaluationType;

//        public BusinessEvaluationUnityNature EvaluationNature;

//        public string RightExpression;//A direct column or database function

//        public string Assertion;
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    public enum BusinessEvaluationUnityType
//    {
//        Equal,
//        GreaterThan,
//        LessThan
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    public enum BusinessEvaluationUnityNature
//    {
//        Live,
//        Session
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public enum BusinessRuleViolation
//    {
//        Fail,
//        Warn,
//        Info,
//        Log
//    }

//    /// <summary>
//    /// Represents specific portions of data like view models, can partially represent a real world class
//    /// </summary>
//    interface IBusinessContext
//    {
//        string Code;

//        string Description;

//        IEnumerable<IBusinessRule> Rules;

//        public void IsValid();

//        public void AddRule();

//        public void RemoveRule();

//        public void EnableRule();

//        public void DisableRule();

//        public void EvaluateRule();
//    }
//}
