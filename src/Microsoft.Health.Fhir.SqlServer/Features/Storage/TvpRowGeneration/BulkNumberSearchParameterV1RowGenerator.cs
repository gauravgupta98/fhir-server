﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Health.Fhir.Core.Features.Search.SearchValues;
using Microsoft.Health.Fhir.SqlServer.Features.Schema.Model;

namespace Microsoft.Health.Fhir.SqlServer.Features.Storage.TvpRowGeneration
{
    internal class BulkNumberSearchParameterV1RowGenerator : BulkSearchParameterRowGenerator<NumberSearchValue, BulkNumberSearchParamTableTypeV1Row>
    {
        public BulkNumberSearchParameterV1RowGenerator(SqlServerFhirModel model, SearchParameterToSearchValueTypeMap searchParameterTypeMap)
            : base(model, searchParameterTypeMap)
        {
        }

        internal override bool TryGenerateRow(short resourceTypeId, string resourceId, short searchParamId, NumberSearchValue searchValue, out BulkNumberSearchParamTableTypeV1Row row)
        {
            bool isSingleValue = searchValue.Low == searchValue.High;

            row = new BulkNumberSearchParamTableTypeV1Row(
                resourceTypeId,
                resourceId,
                searchParamId,
                isSingleValue ? searchValue.Low : null,
                isSingleValue ? null : searchValue.Low ?? (decimal?)VLatest.NumberSearchParam.LowValue.MinValue,
                isSingleValue ? null : searchValue.High ?? (decimal?)VLatest.NumberSearchParam.HighValue.MaxValue);

            return true;
        }
    }
}