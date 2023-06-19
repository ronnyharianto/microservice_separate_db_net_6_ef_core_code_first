using AutoMapper;
using CorePush.Google;
using Falcon.BackEnd.Products.Common;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nancy.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Falcon.BackEnd.Products.Service.Products
{
    public class ProductService : BaseService<ApplicationDbContext>
    {
        private readonly CacheHelper _cacheHelper;
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        public ProductService(ApplicationDbContext dbContext, IMapper mapper, CacheHelper cacheHelper, IOptions<FcmNotificationSetting> settings) : base(dbContext, mapper)
        {
            _cacheHelper = cacheHelper;
            _fcmNotificationSetting = settings.Value;
        }

        public ObjectResult<ProductDto> Create(ProductInput data)
        {
            var retVal = new ObjectResult<ProductDto>(ServiceResultCode.BadRequest);
            var newData = _mapper.Map<ProductInput, Product>(data);

            if (newData != null)
            {
                _dbContext.Products.Add(newData);

                retVal.Obj = _mapper.Map<Product, ProductDto>(newData);
                retVal.OK(null);
            }

            return retVal;
        }
        
        public ObjectResult<List<VariantProductDto>> CreateListVariant(VariantProductInput data)
        {
            var retVal = new ObjectResult<List<VariantProductDto>>(ServiceResultCode.BadRequest);
            var getProductId = _dbContext.Products.Where(p => p.Id == data.ProductId).FirstOrDefault();

            if (getProductId == null) return retVal;

            var newData = _mapper.Map<List<ProductVariant>>(data.ProductVariants);

            if (newData != null)
            {
                foreach(var v in newData)
                {
                    _dbContext.ProductVariants.Add(v);
                    v.ProductId = data.ProductId;
                }

                retVal.Obj = _mapper.Map<List<VariantProductDto>>(newData);
                retVal.OK(null);
            }

            return retVal;
        }

		public ServiceResult DeleteProduct(Guid Id)
		{
			var retval = new ServiceResult(ServiceResultCode.Error);

			var deleteDataProduct = _dbContext.Products.FirstOrDefault(x => x.Id == Id);
			var deleteDataProductVariant = _dbContext.ProductVariants.Where(x => x.ProductId == Id).ToList();

			if (deleteDataProduct != null)
			{
				deleteDataProduct.RowStatus = 1;

				if (deleteDataProductVariant != null)
				{
					foreach (var det in deleteDataProductVariant)
					{
						det.RowStatus = 1;
					}
				}

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult DeleteProductVariant(Guid Id)
		{
			var retval = new ServiceResult(ServiceResultCode.Error);

			var deleteDataProductVariant = _dbContext.ProductVariants.FirstOrDefault(x => x.Id == Id);

			if (deleteDataProductVariant != null)
			{
				deleteDataProductVariant.RowStatus = 1;

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult UpdateProduct(ProductUpdate productUpdate)
		{
			var retval = new ServiceResult(ServiceResultCode.NotFound);

			var searchDataProduct = _dbContext.Products.FirstOrDefault(x => x.Id == productUpdate.Id);

			if (searchDataProduct != null)
			{
				_mapper.Map(productUpdate, searchDataProduct);

				_dbContext.Products.Update(searchDataProduct);

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult UpdateProductVariant(Guid Id, ProductVariantUpdate productVariantUpdate)
		{
			var retval = new ServiceResult(ServiceResultCode.NotFound);

			var updateDataProductVariant = _dbContext.ProductVariants.FirstOrDefault(x => x.Id == Id);

			if (updateDataProductVariant != null)
			{
				updateDataProductVariant.VariantName = productVariantUpdate.VariantName;

				retval.OK(null);
			}
			return retval;
		}

		public ObjectResult<IQueryable<Product>> GetListProducts()
        {
            var cacheData = _cacheHelper.GetCacheData<List<Product>>(ApplicationConstans.CacheKey.ProductData);
            var retVal = new ObjectResult<IQueryable<Product>>(ServiceResultCode.BadRequest);

            if (cacheData != null)
            {
                retVal.Obj = cacheData.AsQueryable();
            }
            else
            {
                retVal.Obj = _dbContext.Products.Include(x => x.ProductVariants).AsQueryable();
                _cacheHelper.SetCacheData(ApplicationConstans.CacheKey.ProductData, retVal.Obj.ToList(), TimeSpan.FromMinutes(5));
            }
            retVal.OK(null);

            return retVal;
        }

        public ObjectResult<IQueryable<Product>> GetListAllProducts()
        {
            var retVal = new ObjectResult<IQueryable<Product>>(ServiceResultCode.BadRequest)
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).IgnoreQueryFilters().AsQueryable()
            };
            retVal.OK(null);

            return retVal;
        }


        public ObjectResult<Product> GetDetailProduct(Guid id)
        {
            var retVal = new ObjectResult<Product>(ServiceResultCode.Ok)
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).Where(x => x.Id == id).FirstOrDefault()
            };

            return retVal;
        }
        
        public ObjectResult<ProductVariant> GetProductVariant(Guid id)
        {
            var retVal = new ObjectResult<ProductVariant>(ServiceResultCode.Ok)
            {
                Obj = _dbContext.ProductVariants.Where(x => x.Id == id).FirstOrDefault()
            };

            return retVal;
        }

        public ObjectResult<NotifDto> CreateNotif(NotifInput input)
        {
            var retVal = new ObjectResult<NotifDto>(ServiceResultCode.BadRequest);

            /* FCM Sender (Android Device) */
            //FcmSettings settings = new FcmSettings()
            //{
            //    ServerKey = _fcmNotificationSetting.ServerKey
            //};

            //HttpClient httpClient = new HttpClient();

            //string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
            //string deviceToken = input.Topic;

            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
            //httpClient.DefaultRequestHeaders.Accept
            //        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var pushMessage = new FcmConfig() 
            //{
            //    Topic = input.Topic,
            //    Notification = new NotifDto() 
            //    {
            //        Title = input.Title,
            //        Body = input.Body
            //    }
            //};          

            //var fcm = new FcmSender(settings, httpClient);
            //var fcmSendResponse = fcm.SendAsync(dat);

            //if (fcmSendResponse.IsCompletedSuccessfully)
            //{
            //    retVal.OK(null);
            //}

            var serverKey = _fcmNotificationSetting.ServerKey;

            var result = "1";

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            tRequest.Method = "post";

            tRequest.ContentType = "application/json";

            var data = new
            {
                to = input.Topic,
                notification = new
                {
                    body = input.Body,
                    title = input.Title
                }
            };

            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(data);

            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));

            //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {

                dataStream.Write(byteArray, 0, byteArray.Length);


                using (WebResponse tResponse = tRequest.GetResponse())
                {

                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {

                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {

                            String sResponseFromServer = tReader.ReadToEnd();

                            result = sResponseFromServer;

                        }
                    }
                }
            }

            if(result != "1")
            {
                retVal.OK(null);
            }

            return retVal;
        }
    }
}
