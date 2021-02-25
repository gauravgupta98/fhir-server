﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Health.Fhir.Core.Features.Search.SearchValues;
using Microsoft.Health.Fhir.SqlServer.Features.Schema.Model;

namespace Microsoft.Health.Fhir.SqlServer.Features.Storage.TvpRowGeneration
{
    internal class BulkReferenceTokenCompositeSearchParameterV1RowGenerator : BulkCompositeSearchParameterRowGenerator<(ReferenceSearchValue component1, TokenSearchValue component2), BulkReferenceTokenCompositeSearchParamTableTypeV1Row>
    {
        private readonly ReferenceSearchParameterV2RowGenerator _referenceRowGenerator;
        private readonly TokenSearchParameterV1RowGenerator _tokenRowGenerator;

        public BulkReferenceTokenCompositeSearchParameterV1RowGenerator(
            SqlServerFhirModel model,
            ReferenceSearchParameterV2RowGenerator referenceRowGenerator,
            TokenSearchParameterV1RowGenerator tokenRowGenerator,
            SearchParameterToSearchValueTypeMap searchParameterTypeMap)
            : base(model, searchParameterTypeMap)
        {
            _referenceRowGenerator = referenceRowGenerator;
            _tokenRowGenerator = tokenRowGenerator;
        }

        internal override bool TryGenerateRow(short resourceTypeId, string resourceId, short searchParamId, (ReferenceSearchValue component1, TokenSearchValue component2) searchValue, out BulkReferenceTokenCompositeSearchParamTableTypeV1Row row)
        {
            if (_referenceRowGenerator.TryGenerateRow(default, searchValue.component1, out var reference1Row) &&
                _tokenRowGenerator.TryGenerateRow(default, searchValue.component2, out var token2Row))
            {
                row = new BulkReferenceTokenCompositeSearchParamTableTypeV1Row(
                    resourceTypeId,
                    resourceId,
                    searchParamId,
                    reference1Row.BaseUri,
                    reference1Row.ReferenceResourceTypeId,
                    reference1Row.ReferenceResourceId,
                    reference1Row.ReferenceResourceVersion,
                    token2Row.SystemId,
                    token2Row.Code);

                return true;
            }

            row = default;
            return false;
        }
    }
}
