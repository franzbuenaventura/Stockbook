using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
//Include the next line if using NUnit

//Include the next line if using VSTS
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//////////////////////////////////////////////////////////////////////////////////////////////
//This version for codeproject.com written by Carl Johansen.
// email: carl at carljohansen dot co dot uk
// web: www.carljohansen.co.uk
//
//Note:  This code is based on the AssertGraphPropertiesEqual method originally posted
//       by Keith Brown (http://www.pluralsight.com/blogs/keith/archive/2005/06/01/9699.aspx).
//
//Revision History:
//1.0   6-Jan-08    CarlJ   Initial version for codeproject.com
//1.1   13-Jan-08   CarlJ   Introduced message parameter and removed redundant reporting of
//                           actual and expected values.
//1.11  13-Jan-08   CarlJ   Improved message format and clarified NUnit support.
//////////////////////////////////////////////////////////////////////////////////////////////

namespace StockbookTests.Class
{
    #region AssertPublicPropertiesEqual related stuff

    /// <summary>
    /// Specifies how the PropertyComparisonExclusion.TargetType property should be used to determine whether to ignore a property during object comparison.
    /// </summary>
    public enum PropertyComparisonExclusionTypeAction
    {
        MatchExactType = 0,
        MatchTypeAndDerivedTypes
    }

    /// <summary>
    /// Represents a property that is to be ignored when deciding whether two instances of a class are equal.
    /// </summary>
    public class PropertyComparisonExclusion
    {
        // Fields
        private string _ignorePropertyName;
        private Type _targetType;
        private PropertyComparisonExclusionTypeAction _typeAction;

        // Methods
        public PropertyComparisonExclusion(Type type, string propertyName)
            : this(type, propertyName, PropertyComparisonExclusionTypeAction.MatchExactType)
        { }

        public PropertyComparisonExclusion(Type type, string propertyName, PropertyComparisonExclusionTypeAction typeAction)
        {
            _targetType = type;
            _ignorePropertyName = propertyName;
            _typeAction = typeAction;
        }

        // Properties
        public string IgnorePropertyName
        {
            get
            {
                return this._ignorePropertyName;
            }
            set
            {
                this._ignorePropertyName = value;
            }
        }

        public Type TargetType
        {
            get
            {
                return this._targetType;
            }

            set
            {
                this._targetType = value;
            }
        }

        public PropertyComparisonExclusionTypeAction TypeAction
        {
            get { return _typeAction; }
            set { _typeAction = value; }
        }
    }

    /// <summary>
    /// Represents a set of properties that are to be ignored when deciding whether two instances of a class are equal.
    /// </summary>
    public class IgnoreProperties : List<PropertyComparisonExclusion>
    {
        /// <summary>
        /// Indicates whether an object property appears in the IgnoreProperties list.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns>If the specified property matches an item in the list, the method returns true.  Otherwise the method returns false.</returns>
        public bool IgnoreProperty(Type type, string propertyName)
        {
            return (base.Find(delegate (PropertyComparisonExclusion item)
            {
                switch (item.TypeAction)
                {
                    case PropertyComparisonExclusionTypeAction.MatchTypeAndDerivedTypes:
                        if (item.IgnorePropertyName == propertyName)
                        {
                            bool currIgnoreItemIsRequiredType = false;
                            Type currType = type;
                            while (currType != null && !(currIgnoreItemIsRequiredType = (item.TargetType == currType)))
                            {
                                currType = currType.BaseType;
                            }
                            return currIgnoreItemIsRequiredType;
                        }
                        else
                        {
                            return false;
                        }
                    case PropertyComparisonExclusionTypeAction.MatchExactType:
                        return (item.TargetType == type) && (item.IgnorePropertyName == propertyName);
                    default:
                        return false; //this should never be reached.
                }
            }) != null);
        }
    }
    #endregion

    public static class UnitTestingHelper
    {
        private static bool StartVisit(object obj, Stack<object> visitedObjects, out bool havePushed)
        {
            havePushed = false;

            // Returns true if the object has already been visited, otherwise adds it to the visited stack.
            // Always returns false for value types and strings.           
            if (obj.GetType().IsValueType || obj is string)
            {
                return false;
            }

            bool haveVisited = false;
            foreach (object currObject in visitedObjects)
            {
                haveVisited = (currObject == (obj as object)); // This weird test forces the use of the base Object.Equals(), because we always want reference equality.
                if (haveVisited)
                    break;
            }

            if (!haveVisited)
            {
                visitedObjects.Push(obj);
                havePushed = true;
            }
            return haveVisited;
        }

        private static void EndVisit(Stack<object> visitedObjects)
        {
            visitedObjects.Pop();
        }

        /// <summary>
        /// Asserts that the public properties of two objects are equal (a deep comparison).
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the unit test expects.</param>
        /// <param name="actual">The second object to compare. This is the object the unit test produced.</param>
        public static void AssertPublicPropertiesEqual(object expected, object actual)
        {
            AssertPublicPropertiesEqual(expected, actual, null, null, null, new Stack<object>());
        }

        /// <summary>
        /// Asserts that the public properties of two objects are equal (a deep comparison).
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the unit test expects.</param>
        /// <param name="actual">The second object to compare. This is the object the unit test produced.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void AssertPublicPropertiesEqual(object expected, object actual, string message)
        {
            AssertPublicPropertiesEqual(expected, actual, null, message, null, new Stack<object>());
        }

        /// <summary>
        /// Asserts that the public properties of two objects are equal (a deep comparison).
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the unit test expects.</param>
        /// <param name="actual">The second object to compare. This is the object the unit test produced.</param>
        /// <param name="ignoreProperties">An <see cref="IgnoreProperties"/> object that specifies the properties that are to be ignored during the comparison.</param>
        public static void AssertPublicPropertiesEqual(object expected, object actual, IgnoreProperties ignoreProperties)
        {
            AssertPublicPropertiesEqual(expected, actual, null, null, ignoreProperties, new Stack<object>());
        }

        /// <summary>
        /// Asserts that the public properties of two objects are equal (a deep comparison).
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the unit test expects.</param>
        /// <param name="actual">The second object to compare. This is the object the unit test produced.</param>
        /// <param name="ignoreProperties">An <see cref="IgnoreProperties"/> object that specifies the properties that are to be ignored during the comparison.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void AssertPublicPropertiesEqual(object expected, object actual, IgnoreProperties ignoreProperties, string message)
        {
            AssertPublicPropertiesEqual(expected, actual, null, message, null, new Stack<object>());
        }

        internal static void AssertPublicPropertiesEqual(object expected, object actual, string objectDescription, string message, IgnoreProperties ignoreProperties, Stack<object> visitedObjects)
        {
            if (String.IsNullOrEmpty(objectDescription))
            {
                objectDescription = "<object>";
            }
            string assertMsg = (String.IsNullOrEmpty(objectDescription) ? String.Empty : "(Property: " + objectDescription + ")");
            if (!String.IsNullOrEmpty(message))
            {
                assertMsg += (assertMsg.Length > 0 ? " " : String.Empty) + message;
            }

            if ((expected == null) || (actual == null))
            {
                // Either expected or actual is null, so assert that both are.
                Assert.AreEqual(expected, actual, assertMsg);
            }
            else
            {
                //Neither expected nor actual is null.
                bool haveAddedToVisitedObjects;
                if (StartVisit(expected, visitedObjects, out haveAddedToVisitedObjects))
                    //Looks like the caller's original type contains a circular reference - we have already seen [expected].
                    return;

                //Assert that expected and actual are of the same type.
                Assert.AreSame(expected.GetType(), actual.GetType(), String.Format("Objects are not of the same type. {0}", assertMsg));
                Type objectType = expected.GetType();

                bool checkObjectPublicProperties = !(objectType.IsPrimitive || objectType.IsEnum || (expected is string) || expected is DateTime); // For these types there is no need to check the public properties.
                bool isValueTypeObjectWithoutRefProperties = objectType.IsValueType; // While checking public properties, we will keep track of whether the object is a value type that contains only value type properties (in which case we will check bitwise equality).

                if (checkObjectPublicProperties)
                {
                    //See if the caller has supplied an ignore list.
                    bool hasIgnoreList = (ignoreProperties != null) && (ignoreProperties.Count > 0);

                    foreach (PropertyInfo currProperty in objectType.GetProperties())
                    {
                        isValueTypeObjectWithoutRefProperties &= currProperty.PropertyType.IsValueType;

                        MethodInfo getterMethod = currProperty.GetGetMethod();
                        if (getterMethod != null)
                        {
                            bool isStaticProperty = getterMethod.IsStatic;
                            bool isIndexedProperty = (getterMethod.GetParameters().Length > 0);

                            if ((!isStaticProperty) && (!isIndexedProperty) && (!hasIgnoreList || !ignoreProperties.IgnoreProperty(objectType, currProperty.Name)))
                            {
                                //This is not a static property, not an indexed property and it is not on the ignore list so check it.
                                object expectedPropValue = currProperty.GetValue(expected, null);
                                object actualPropValue = currProperty.GetValue(actual, null);
                                string propertyDescription = (objectDescription == null) ? currProperty.Name : String.Format("{0}.{1}", objectDescription, currProperty.Name);

                                AssertPublicPropertiesEqual(expectedPropValue, actualPropValue, propertyDescription, message, ignoreProperties, visitedObjects);
                            } // is non-indexed and not on ignore list 
                        } // has a Get method
                    } // foreach property in object

                    if (typeof(IEnumerable).IsAssignableFrom(objectType))
                    {
                        //Object is some kind of collection.  Enumerate through all the objects it contains.
                        IEnumerator it_e = ((IEnumerable)expected).GetEnumerator();
                        IEnumerator it_a = ((IEnumerable)actual).GetEnumerator();
                        int count = 0;
                        bool moreItemsInExpected = true;
                        while (moreItemsInExpected)
                        {
                            if (moreItemsInExpected = it_e.MoveNext())
                            {
                                Assert.IsTrue(it_a.MoveNext(), String.Format("Expected more items in collection; actual has only {0} item(s). {1}", count, assertMsg));
                                count++;
                                AssertPublicPropertiesEqual(it_e.Current, it_a.Current, String.Format("{0}[{1}]", objectDescription, count - 1), message, ignoreProperties, visitedObjects);
                            }
                            else
                            {
                                Assert.IsFalse(it_a.MoveNext(), String.Format("Expected {0} items in collection, but actual has more than that. {1}", count, assertMsg));
                            }
                        } // while there are more items in the enumeration
                    } // object is enumerable                
                } // check object's public properties

                bool checkObjectValueEquality = (isValueTypeObjectWithoutRefProperties || (expected is string));
                if (checkObjectValueEquality)
                {
                    Assert.AreEqual(expected, actual, assertMsg);
                }
                if (haveAddedToVisitedObjects)
                {
                    EndVisit(visitedObjects);
                }
            } // object is not null
        }
    }
}


