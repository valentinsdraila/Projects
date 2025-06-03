using Microsoft.ML;
using RecommendationService.DataLayer;
using RecommendationService.Model;

namespace RecommendationService.ServiceLayer
{
    public class ModelManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _modelPath = "recommendations.zip";
        private readonly MLContext _mlContext;
        private readonly ModelTrainer _trainer;
        private ITransformer _model;

        public ModelManager(MLContext mlContext, ModelTrainer trainer, IServiceProvider serviceProvider)
        {
            _mlContext = mlContext;
            _trainer = trainer;
            _serviceProvider = serviceProvider;
        }

        public async Task TrainAndSaveModelAsync()
        {
            var data = await _trainer.PrepareTrainingData();
            _model = _trainer.TrainModel(data);
            _mlContext.Model.Save(_model, data.Schema, _modelPath);
        }

        public void LoadModel()
        {
            using var fileStream = File.OpenRead(_modelPath);
            _model = _mlContext.Model.Load(fileStream, out _);
        }
        public class PredictionScore
        {
            public float Score { get; set; }
        }
        public class Recommendation
        {
            public Guid Id { get; set; }
            public float Score { get; set; }
        }

        public async Task<List<Recommendation>> PredictTopProductsForUser(Guid userId, int numberOfProducts)
        {
            var predictions = new List<(Guid productId, float score)>();

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IUserInteractionRepository>();
            var productIds = await repository.GetDistinctProductIds();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserInteraction, PredictionScore>(_model);

            foreach (var productId in productIds)
            {
                var input = new UserInteraction
                {
                    UserId = Mapper.ConvertGuidToUInt(userId),
                    ProductId = Mapper.ConvertGuidToUInt(productId)
                };


                var prediction = predictionEngine.Predict(input);
                if (float.IsNaN(prediction.Score) || float.IsInfinity(prediction.Score))
                    prediction.Score = 0f;

                predictions.Add((productId, prediction.Score));
            }

            return predictions
                .OrderByDescending(p => p.score)
                .Take(numberOfProducts)
                .Select(p => new Recommendation { Id = p.productId, Score = p.score  })
                .ToList();
        }

    }

}
