using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;
using Sabio.Data;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.ExternalLinks;
using Stripe;

namespace Sabio.Services
{
    public class ExternalLinkService : IExternalLinkService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        public ExternalLinkService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookUpService = lookUp;
        }
        #region Delete
        public void Delete(int id)
        {
            string procName = "[dbo].[ExternalLinks_Delete_ById]";
            ExternalLink eLink = new ExternalLink();
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        #endregion

        #region SelectByCreatedBy
        public List<ExternalLink> GetSelectByCreatedBy(int userId)
        {

            string procName = "[dbo].[ExternalLinks_SelectByCreatedBy]";

            List<ExternalLink> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramsCollection)
            {
                paramsCollection.AddWithValue("@UserId", userId);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ExternalLink eLink = MapSingleELink(reader, ref startingIndex);
                if (list == null)
                {
                    list = new List<ExternalLink>();
                }
                list.Add(eLink);
            }
            );
            return list;
        }
        #endregion

        #region Update
        public void Update(ExternalLinkUpdateRequest model, int Id)
        {
            string procName = "[dbo].[ExternalLinks_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

                CommonParams(model, col, Id);

                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }
        #endregion

        #region Insert
        public int Add(ExternalLinkAddRequest model, int Id)
        {
            int id = 0;

            string procName = "[dbo].[ExternalLinks_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

                CommonParams(model, col, Id);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });
            return id;

        }
        #endregion

        #region Map
        private ExternalLink MapSingleELink(IDataReader reader, ref int startingIndex)
        {
            ExternalLink eLink = new ExternalLink();


            eLink.Id = reader.GetSafeInt32(startingIndex++);
            eLink.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            eLink.UrlTypeId = new LookUp();
            eLink.UrlTypeId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            eLink.Url = reader.GetSafeString(startingIndex++);
            eLink.EntityId = reader.GetSafeInt32(startingIndex++);
            eLink.EntityTypeId = new LookUp();
            eLink.EntityTypeId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            eLink.DateCreated = reader.GetSafeDateTime(startingIndex++);
            eLink.DateModified = reader.GetSafeDateTime(startingIndex++);

            return eLink;
        }
        #endregion

        #region Params
        private static void CommonParams(ExternalLinkAddRequest model, SqlParameterCollection col, int userId)
        {

            col.AddWithValue("@UserId", userId);
            col.AddWithValue("@UrlTypeId", model.UrlTypeId);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@EntityId", model.EntityId);
            col.AddWithValue("@EntityTypeId", model.EntityTypeId);
        }
        #endregion
    }
}
