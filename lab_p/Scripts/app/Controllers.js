

app.controller('matrixController', function ($scope,$http, APIService) {
    //$scope.file = {}; //Модель
    //$scope.options =
    //    {
    //        //Вызывается для каждого выбранного файла
    //        change: function (file) {
    //            //В file содержится информация о файле
    //            //Загружаем на сервер
    //            var servCall = APIService.multiplyMatrixFromFile(file);
    //            console.log(file);
    //            servCall.then(function (d) {
    //                $scope.data = d;
    //            }, function (error) {
    //                console.log(error);
    //                $scope.errors = parseErrors(error);
    //            });
    //        }
    //    }
    var formdata = [];
    $scope.data =[];
    $scope.getTheFiles = function ($files) {
        console.log($files)
        angular.forEach($files, function (value, key) {           
            console.log(formdata);
            var a = new FormData();
            a.append(key, value);
            formdata.push(a);
        });
        console.log(formdata);
    };


    // NOW UPLOAD THE FILES.
    
    $scope.uploadFiles = function () {
        formdata.forEach(function (item, i, formdata) {
            
            var request = {
                method: 'POST',
                url: '/api/values/',
                data: item,
                headers: {
                    'Content-Type': undefined
                }
            };

            // SEND THE FILES.
            var now1 = new Date().getMilliseconds();
            $http(request).then(function (d) {
                var now2 = new Date().getMilliseconds();
                $scope.data.push({ data: d.data, time: Math.abs(now2 - now1) });
            }, function (error) {
                console.log(error);
                $scope.errors = parseErrors(error);
            });
        });
       
    }
    $scope.getMatrix = function () {
        var filenames = [];
        var data = $scope.data;
        data.forEach(function (item, i,data) {
            filenames.push(item.data[0].filename.split('.')[0]);
        });
        console.log(filenames);
        $scope.data1 = [];   
        filenames.forEach(function (item, i, filenames) {
            
            var request = {
                method: 'GET',
                url: '/api/values/' + item
            };

            // SEND THE FILES.
            var now1 = new Date().getMilliseconds();
            $http(request).then(function (d) {
                var now2 = new Date().getMilliseconds();
                $scope.data1.push({ data: d.data, time: Math.abs(now2 - now1) });
            }, function (error) {
                console.log(error);
                $scope.errors = parseErrors(error);
            });
        });
        
            
        
       
    }
});