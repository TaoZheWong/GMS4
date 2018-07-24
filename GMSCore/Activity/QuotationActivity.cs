using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class QuotationActivity : ActivityBase
    {
        public QuotationActivity()
        {
        }

        
        public ResultType DeleteRecipe(Recipe recipe, LogSession session)
        {
            if (session == null)
                throw new NullSessionException();

            recipe.Delete();
           
            return ResultType.Ok;
        }

        public ResultType DeleteRecipeDetail(RecipeDetail recipeDetail, LogSession session)
        {
            if (session == null)
                throw new NullSessionException();

            recipeDetail.Delete();
          
            return ResultType.Ok;
        }

        public Recipe RetrieveRecipeByRecipeNo(short companyID, string recipeNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Recipe.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Recipe.RecipeNo"),
                               helper.CleanValue(recipeNo));

            return Recipe.RetrieveFirst(stb.ToString());
        }

        public IList<RecipeDetail> RetrieveRecipeDetailByRecipeNo(short companyID, string recipeNo)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("RecipeDetail.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("RecipeDetail.RecipeNo"),
                               helper.CleanValue(recipeNo));


            return RecipeDetail.RetrieveQuery(stb.ToString());
        }
        

    }
}
