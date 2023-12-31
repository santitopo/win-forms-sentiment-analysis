﻿using System;
using Domain;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace Tests
{
    [TestClass]
    public class EntityLogicTests
    {
        EntityLogic subsystem;
        Repository repository;

        [TestInitialize]
        public void SetUp()
        {
            repository = new Repository();
            subsystem = new EntityLogic(repository);
            subsystem.DeleteAllEntities();
        }

        [TestCleanup]
        public void CleanUp()
        {
            subsystem.DeleteAllEntities();
        }

        [TestMethod]
        public void addEntity()
        {
            Entity e = new Entity("Coca-Cola");
            subsystem.AddEntity(e);
            CollectionAssert.Contains(subsystem.GetEntitites, e);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException),
            "no es posible agregar la misma entidad o una subsecuencia de una entidad que ya se encuentra " +
            "en la lista")]
        public void AddSameEntity()
        {
            Entity e = new Entity("cocA-Cola");
            subsystem.AddEntity(e);
            Entity e2 = new Entity("CoCa-cola");
            subsystem.AddEntity(e);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException),
            "no es posible agregar la misma entidad o una subsecuencia de una entidad que ya se encuentra " +
            "en la lista")]
        public void AddSubstringEntity()
        {
            Entity e = new Entity("Coca-Cola");
            subsystem.AddEntity(e);
            Entity e2 = new Entity("Coca-Cola2");
            subsystem.AddEntity(e);
        }

        [TestMethod]
        public void GetEntitiesTest()
        {
            Entity e = new Entity("Coca-Cola");
            Entity e2 = new Entity("Fanta");
            subsystem.AddEntity(e);
            subsystem.AddEntity(e2);

            Assert.AreEqual(subsystem.GetEntitites.Length, 2);
            CollectionAssert.Contains(subsystem.GetEntitites, e);
            CollectionAssert.Contains(subsystem.GetEntitites, e2);
        }

        [TestMethod]
        public void getEntityByNameTest()
        {
            Entity e = new Entity("Coca-Cola");
            Entity e2 = new Entity("Fanta");
            subsystem.AddEntity(e);
            subsystem.AddEntity(e2);

            Entity expectedEntity = subsystem.GetEntityByName(e.Name);
            Assert.AreEqual(e, expectedEntity);
        }

        [TestMethod]
        public void deleteEntity()
        {
            Entity e = new Entity("Coca-Cola");
            subsystem.AddEntity(e);
            subsystem.DeleteEntity(e);
            CollectionAssert.DoesNotContain(subsystem.GetEntitites, e);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
           "no es posible eliminar de una lista vacía")]
        public void deleteEntityOfAnEmptyList()
        {
            Entity e = new Entity("Coca-Cola");
            subsystem.DeleteEntity(e);
        }
    }
}
