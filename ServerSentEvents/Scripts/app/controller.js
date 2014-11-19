angular.module('sseApp', []).controller('sseController', ['$scope', '$http', function ($scope, $http) {
    $scope.message = "te";
    $scope.script = { name: "Ville Virtanen" };
    $scope.isAlertVisible = false;
    $scope.isListening = false;

    $scope.startProcess = function () {
        $scope.message = "";
        $scope.isAlertVisible = false;
        $http.post("http://localhost:53193/api/process", JSON.stringify($scope.script));
    };

    $scope.startListening = function () {
        $scope.isListening = true;
        if (!!window.EventSource) {
            eventSource = new EventSource('http://localhost:53193/api/StatusUpdates?sessionId=' + $scope.script.sessionId);
            eventSource.addEventListener('message', function (e) {
                $scope.message = e.data;
                $scope.isAlertVisible = true;
                $scope.$apply();
            }, false);
            eventSource.addEventListener('open', function (e) {
                console.log("open!");
            }, false);
            eventSource.addEventListener('error', function (e) {
                if (e.readyState == EventSource.CLOSED) {
                    console.log("error!");
                }
            }, false);
        }
    };

    $scope.stopListening = function () {
        var url = "http://localhost:53193/api/StatusUpdates/" + $scope.script.sessionId;
        $http.delete(url).success(function () {
            if (eventSource != null) {
                eventSource.close();
            }
            $scope.isListening = false;
        });
    };

    var eventSource;
    
    function getSessionId() {
        var id = "G" + Math.random();
        return id.substring(3, 8);
    }

    function init() {
        $scope.script.sessionId = getSessionId();
    }

    init();

}]);