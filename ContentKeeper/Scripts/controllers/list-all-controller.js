angular.module('contentKeeper', [])
    .service('listAllService', function($http) {

        return({
            listAll: listAll
        });

        function listAll() {
            var request = $http.get('/api/Content/');

            return (request.then());
        }
    })
    .controller('listAllController', [
            '$scope', 'listAllService', function ($scope, listAllService) {
                listAllService.listAll().then(
                    function (data) {
                        $scope.entries = data;
                    }
                );
            }
    ]);