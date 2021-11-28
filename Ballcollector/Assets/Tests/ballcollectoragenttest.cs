using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class ballcollectoragenttest
    {
        [SetUp]
        public void LoadScene()
        {
            SceneManager.LoadScene("Ballcollector");
        }

        /// <summary>
        /// Checks goal object are created 
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ballcollectoragenttesttocheckgoaltag()
        {

            var goalObject = GameObject.FindGameObjectsWithTag("goal");
            Assert.IsNotNull(goalObject);

            yield return null;
        }

        /// <summary>
        /// Chceks agent is created
        /// </summary>
        /// <returns></returns>

        [UnityTest]
        public IEnumerator ballcollectoragenttesttocheckAgent()
        {
            var agentObj = GameObject.Find("SphereAgent");
            Assert.IsNotNull(agentObj);

            yield return null;
        }

        /// <summary>
        /// Check collision between agent and goal. If the goal is not present then the test is success
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator agentcollidinggoal()
        {
            GameObject goalObj = GameObject.FindGameObjectWithTag("goal");
            goalObj.transform.position = Vector3.zero;
            GameObject agentObj = GameObject.Find("SphereAgent");
            agentObj.transform.position = Vector3.zero;
            yield return new WaitForSeconds(1f);
            UnityEngine.Assertions.Assert.IsNull(goalObj);
            yield return null;
        }
    }
}
