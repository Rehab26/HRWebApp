using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Core;

namespace HRWebApp.Models
{
    public class ResponseModel
    {
            #region Variables

            public int RowsCount { get; set; }
            public int PageSize { get; set; }
            /// <summary>
            ///     This attribute for OAuth, when logging and 
            ///     need to redirect to specific page after sucess login.
            /// </summary>
            public string RedirectUrl { get; set; }
            #endregion

            public ResponseModel()
            {
                RowsCount = 0;
            }
    }


        public class ResponseModel<T> : ResponseModel
    {
            #region Variables

            public T Data { get; set; }

            #endregion

            #region Constructor

            public ResponseModel(T data = default(T))
                : base()
            {
                Data = data;
            }
            #endregion
        }
    }
