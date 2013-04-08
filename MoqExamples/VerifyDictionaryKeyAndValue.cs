using System.Collections.Generic;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MoqExamples
{
    public class When_you_verify_the_dictionary_has_certain_keys_and_values
    {
        static Mock<IConsume> _consumer;
        static ConsumerUser _consumerUser;
        static string _username;
        static string _password;

        Establish context = () =>
            {
                _username = "Dorothy";
                _password = "Perkins";
                _consumer = new Mock<IConsume>();
                _consumerUser = new ConsumerUser(_consumer.Object);
            };

        Because it_consumes = () => _consumerUser.Consume(_username, _password);

        It should_have_username_and_password_in_the_input = () => 
            _consumer.Verify(d =>d.Consume(Moq.It.Is<Dictionary<string, string>>(
                    dict => dict["username"] == "Dorothy" &&
                            dict["password"] == "Perkins")));
}

    public interface IConsume
    {
        void Consume(Dictionary<string, string> input);
    }

    public class ConsumerUser 
    {
        private readonly IConsume _consume;

        public ConsumerUser(IConsume consume)
        {
            _consume = consume;
        }

        public void Consume(string username, string password)
        {
            var pairs = new Dictionary<string, string>();
            pairs.Add("username", username);
            pairs.Add("password", password);

            _consume.Consume(pairs);
        }
    }
}