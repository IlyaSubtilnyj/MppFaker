using DataTransferObject;
using FakerTests.Formulators;

namespace FakerTests
{
    [TestClass]
    public class FakerTests
    {

        Faker faker = new Faker();

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {

            Composer.Add(typeof(ListWithOneGenericParameter<>));
            Composer.Add(typeof(ListIntSpecification42));
            Composer.Add(typeof(DictionaryMoreIntIntOriented<>));
            Composer.Add(typeof(DictionaryLessIntIntOriented<>));
        }

        [TestMethod]
        [ExpectedException(typeof(UnresolvableRecursionException))]
        public void Faker_WhenRecursionInConstructor_ThenThrowUnresolvableRecursionException()
        {

            faker.Create<DTOs.SelfRecursiveInCtorDTO>();
        }

        [TestMethod]
        [ExpectedException(typeof(UnresolvableRecursionException))]
        public void Faker_WhenRecursionInPublicField_ThenThrowUnresolvableRecursionException()
        {

            faker.Create<DTOs.AThatReferToBInCtorDTO>();
        }

        [TestMethod]
        [ExpectedException(typeof(UnresolvableRecursionException))]
        public void Faker_WhenRecursionInPublicProperty_ThenThrowUnresolvableRecursionException()
        {

            faker.Create<DTOs.CThatReferToDInCtorDTO>();
        }

        [TestMethod]
        public void Faker_WhenRecursionInPrivateField_ShouldNotThrowException()
        {
            try
            {
                faker.Create<DTOs.DtoThatReferToItselfInPrivateField>();
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception was thrown: {ex.Message}");
            }  
        }

        [TestMethod]
        public void Faker_WhenNotDtoPassed_ShouldReturnDefault()
        {

            Assert.AreEqual(faker.Create<int>(), default(int));
            Assert.IsNull(faker.Create<TestDoubles.NotDTO>());
        }

        [TestMethod]
        public void Composer_WhenSpecificationExists_ShouldChooseSpecification()
        {

            List<int> actual = (List<int>)Composer.Compose(typeof(List<int>));
            List<int> expected = new List<int> { 42 };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Composer_WhenPartialSpecificationParameters_ShouldNotFail()
        {
    
            try
            {

                var expected = new Dictionary<long, int> { { default(int), default(int) + 1 } };
                object actual = Composer.Compose(typeof(Dictionary<long, int>));
                Assert.IsInstanceOfType(actual, typeof(Dictionary<long, int>));
                CollectionAssert.AreEqual(expected, (Dictionary<long, int>)actual);
            }
            catch (Exception ex)
            {

                Assert.Fail($"An exception was thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void Composer_WhenTwoCandidateExistsWithDifferentParametersOrder_ShouldChooseOneWithEarlierSpecification()
        {

            try
            {

                var expected = new Dictionary<int, int> { { default(int) + 1, default(int) } };
                object actual = Composer.Compose(typeof(Dictionary<int, int>));
                Assert.IsInstanceOfType(actual, typeof(Dictionary<int, int>));
                CollectionAssert.AreEqual(expected, (Dictionary<int, int>)actual);
            }
            catch (Exception ex)
            {

                Assert.Fail($"An exception was thrown: {ex.Message}");
            }
        }
    }

}