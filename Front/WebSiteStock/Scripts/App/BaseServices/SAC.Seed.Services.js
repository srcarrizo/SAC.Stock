stockApp.factory('queryInfoService', function () {
    return {
        ReturnQueryInfo: function (pageIndex, pageSize, sortColumn, sortDirection, filterSpec, filterValue) {
            var sortInfo = { Field: sortColumn, Direction: sortDirection };
            var filterInfo = { Spec: filterSpec, Value: filterValue };

            var QueryInfo =
                {
                    PageIndex: pageIndex,
                    PageSize: pageSize,
                    SortInfo:
                    [
                        sortInfo,
                    ],
                    FilterInfo: filterInfo
                };

            return QueryInfo;
        },
        ReturnInitialQueryInfo: function () {
            var sortInfo = { Field: 'Default', Direction: 'Asc' };
            var filterInfo = { Spec: 'FullSearch', Value: null };

            var QueryInfo =
                {
                    PageIndex: 1,
                    PageSize: 1000,
                    SortInfo:
                    [
                        sortInfo
                    ],
                    FilterInfo: null
                };

            return QueryInfo;
        }
    };
});

stockApp.factory('currentUser', function () {
    var currentUser = null;
    return {
        SetUser: function (value) {
            currentUser = value;
            return currentUser;
        },
        GetUset: function () {
            return currentUser;
        }
    };
});