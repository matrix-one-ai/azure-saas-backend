﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using Abp;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using MiniExcelLibs;
using Icon.Authorization.Users;
using Icon.Authorization.Users.Importing.Dto;

namespace Icon.DataExporting.Excel.MiniExcel
{
    public abstract class MiniExcelExcelImporterBase<TEntity>(ILocalizationManager localizationManager)
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(IconConsts.LocalizationSourceName);

        protected List<TEntity> ProcessExcelFile(byte[] fileBytes, Func<dynamic, TEntity> processExcelRow,
            bool useOldExcelFormat = false)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                var rows = stream.Query(useHeaderRow:true).ToList();
                foreach (var row in rows)
                {
                    var entitiesInWorksheet = ProcessWorksheet(row, processExcelRow);
                    entities.AddRange(entitiesInWorksheet);
                }
            }

            return entities;
        }

        private List<TEntity> ProcessWorksheet(dynamic row, Func<dynamic, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            try
            {
                var entity = processExcelRow(row);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }
            catch (Exception)
            {
                //ignore
            }

            return entities;
        }
        
        protected string GetRequiredValueFromRowOrNull(
            dynamic row,
            string columnName,
            StringBuilder exceptionMessage)
        {
            var cellValue = (row as ExpandoObject).GetOrDefault(columnName)?.ToString();
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            throw new AbpException(GetLocalizedExceptionMessagePart(columnName));
        }

        protected object GetOptionalValueFromRowOrNull<T>(dynamic row, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = (row as ExpandoObject).GetOrDefault(columnName)?.ToString();
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            return default(T);
        }
        
        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsRequired", _localizationSource.GetString(parameter)) + "; ";
        }
        
        // Possible types: Int32, Long, Guid, String
        // PowerTools:ConvertToRequiredPrimaryKey
        protected object ConvertToRequiredPrimaryKey<T>(string value)
        {
            return typeof(T).Name switch
            {
                "Int32" => Convert.ToInt32(value),
                "Long" => Convert.ToInt64(value),
                "Guid" => Guid.Parse(value),
                "String" => value,
                _ => default(T)
            };
        }
    }
}