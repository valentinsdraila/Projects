using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using RecommendationService.DataLayer;
using RecommendationService.Model;

namespace RecommendationService.ServiceLayer
{
    public class ModelTrainer
    {
        private readonly IServiceProvider serviceProvider;
        private readonly MLContext _mlContext;

        public ModelTrainer(IServiceProvider serviceProvider, MLContext mlContext)
        {
            this.serviceProvider = serviceProvider;
            _mlContext = mlContext;
        }

        public async Task<IDataView> PrepareTrainingData()
        {

            using (var scope = serviceProvider.CreateScope())
            {
                var userInteractionRepository = scope.ServiceProvider.GetRequiredService<IUserInteractionRepository>();
                var entities = await userInteractionRepository.GetAll();
                var interactions = Mapper.MapToTrainerInput(entities);
                return _mlContext.Data.LoadFromEnumerable(interactions);
            }
        }
        public ITransformer TrainModel(IDataView trainingData)
        {
            var pipeline = _mlContext.Transforms
                .Conversion.MapValueToKey("UserId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("ProductId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserId",
                    MatrixRowIndexColumnName = "ProductId",
                    LabelColumnName = "Label",
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));

            var model = pipeline.Fit(trainingData);
            return model;
        }

    }
}
