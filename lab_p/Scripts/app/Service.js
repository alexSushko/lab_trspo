app.directive('ngFiles', ['$parse', function ($parse) {

    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

app.service("APIService", function ($http) {

    //Owners

    this.multiplyMatrixFromFile = function (files) {
        return $http({
            method: 'post',
            data: files,
            url: '/api/values'
        })
    },
        this.multiplyMatrixFromServer = function (name, page, items, filter) {

        return $http({
            method: 'get',
            url: '/api/owner/find/' + name.toLowerCase() + '/' + page + '/' + items + '/' + filter
        })
    }
    
    
}); 

    
