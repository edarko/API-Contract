using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace APIExercise
{
    public class Tests
    {
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.thecatapi.com/");
            client.DefaultRequestHeaders.Add("x-api-key", "DEMO-API-KEY");
        }

        [TestCase]
        public void getVotesReturns200ResponseCode()
        {
            var response = client.GetAsync("v1/votes").GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;

            List<VotesDTO> responseObject = JsonConvert.DeserializeObject<List<VotesDTO>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Greater(responseObject.Count, 0);
        }

        [TestCase]
        public void getVotesByIdReturns200ResponseCode()
        {
            var response = client.GetAsync("v1/votes").GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;

            List<VotesDTO> responseObject = JsonConvert.DeserializeObject<List<VotesDTO>>(responseBody);

            Random rdm = new Random();
            var randomIndex = rdm.Next(0, responseObject.Count);

            var vote = responseObject[randomIndex].id;

            var oldVoteObject = responseObject[randomIndex];

            var responseById = client.GetAsync($"v1/votes/{vote}").GetAwaiter().GetResult();

            var voteBody = responseById.Content.ReadAsStringAsync().Result;

            VotesDTO newVoteByIdResponse = JsonConvert.DeserializeObject<VotesDTO>(voteBody);

            Assert.AreEqual(oldVoteObject.id, newVoteByIdResponse.id);
            Assert.AreEqual(oldVoteObject.sub_id, newVoteByIdResponse.sub_id);
            Assert.AreEqual(oldVoteObject.value, newVoteByIdResponse.value);
        }

        [TestCase]
        public void postVotesReturns201ResponseCode()
        {
            VotesDTO vote = new VotesDTO();
            vote.image_id = "c8k";
            vote.sub_id = "demo-0.08750269135758182";
            vote.value = "1";

            string output = JsonConvert.SerializeObject(vote);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            var response = client.PostAsync("v1/votes", content).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            VotesDTO responseObject = JsonConvert.DeserializeObject<VotesDTO>(responseBody);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(true, responseObject.id);
            Assert.IsNotNull("SUCCESS", responseObject.message);
        }

        [TestCase]
        public void getNewlyCreateVoteMatchesVoteGetById()
        {
            VotesDTO vote = new VotesDTO();
            vote.image_id = "c8k";
            vote.sub_id = "demo-0.08750269135758182";
            vote.value = "1";

            string output = JsonConvert.SerializeObject(vote);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            var response = client.PostAsync("v1/votes", content).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            VotesDTO responseVote = JsonConvert.DeserializeObject<VotesDTO>(responseBody);

            var responseById = client.GetAsync($"v1/votes/{responseVote.id}").GetAwaiter().GetResult();

            var voteBody = responseById.Content.ReadAsStringAsync().Result;

            VotesDTO getVoteByIdResponse = JsonConvert.DeserializeObject<VotesDTO>(voteBody);

            Assert.AreEqual(responseVote.id, getVoteByIdResponse.id);
            Assert.AreEqual(responseVote.sub_id, getVoteByIdResponse.sub_id);
            Assert.AreEqual(responseVote.value, getVoteByIdResponse.value);
        }

        [TestCase]
        public void deleteVoteByIdReturnsSuccessMessage()
        {
            VotesDTO vote = new VotesDTO();
            vote.image_id = "c8k";
            vote.sub_id = "demo-0.08750269135758182";
            vote.value = "1";

            string output = JsonConvert.SerializeObject(vote);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            var response = client.PostAsync("v1/votes", content).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            VotesDTO responseVote = JsonConvert.DeserializeObject<VotesDTO>(responseBody);

            var deleteResponse = client.DeleteAsync($"v1/votes/{responseVote.id}").GetAwaiter().GetResult();
            var deleteResponseBody = deleteResponse.Content.ReadAsStringAsync().Result;
            VotesDTO deletedVote = JsonConvert.DeserializeObject<VotesDTO>(deleteResponseBody);

            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.IsNotNull("SUCCESS", deletedVote.message);
        }

        [TestCase]
        public void getADeletedVoteByIdReturns404ResponseCode()
        {
            VotesDTO vote = new VotesDTO();
            vote.image_id = "c8k";
            vote.sub_id = "demo-0.08750269135758182";
            vote.value = "1";

            string output = JsonConvert.SerializeObject(vote);
            var content = new StringContent(output, Encoding.UTF8, "application/json");

            var response = client.PostAsync("v1/votes", content).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            VotesDTO responseVote = JsonConvert.DeserializeObject<VotesDTO>(responseBody);

            client.DeleteAsync($"v1/votes/{responseVote.id}").GetAwaiter().GetResult();

            var getVoteByIdResponse = client.GetAsync($"v1/votes/{responseVote.id}").GetAwaiter().GetResult();
            var getVoteByIdResponseBody = getVoteByIdResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, getVoteByIdResponse.StatusCode);
            Assert.IsNotNull("NOT_SUCCESS", getVoteByIdResponseBody);
        }
    }
}