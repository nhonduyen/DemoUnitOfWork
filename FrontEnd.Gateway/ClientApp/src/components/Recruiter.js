"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var react_router_dom_1 = require("react-router-dom");
var CandidatesStore = require("../store/Candidates");
var Recruiter = /** @class */ (function (_super) {
    __extends(Recruiter, _super);
    function Recruiter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    // This method is called when the component is first added to the document
    Recruiter.prototype.componentDidMount = function () {
        this.ensureDataFetched();
    };
    // This method is called when the route parameters change
    Recruiter.prototype.componentDidUpdate = function () {
        this.ensureDataFetched();
    };
    Recruiter.prototype.render = function () {
        return (React.createElement(React.Fragment, null,
            React.createElement("h1", { id: "tabelLabel" }, "Candidates"),
            this.renderCandidatesTable()));
    };
    Recruiter.prototype.ensureDataFetched = function () {
        var startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
        this.props.requestCandidates(startDateIndex);
    };
    Recruiter.prototype.renderCandidatesTable = function () {
        return (React.createElement("table", { className: 'table table-striped', "aria-labelledby": "tabelLabel" },
            React.createElement("thead", null,
                React.createElement("tr", null,
                    React.createElement("th", null, "Id"),
                    React.createElement("th", null, "Name"),
                    React.createElement("th", null, "Recuiter Id"))),
            React.createElement("tbody", null, this.props.candidates.map(function (candidate) {
                return React.createElement("tr", { key: candidate.id },
                    React.createElement("td", null, candidate.id),
                    React.createElement("td", null, candidate.name),
                    React.createElement("td", null, candidate.recruiterId));
            }))));
    };
    Recruiter.prototype.renderPagination = function () {
        var prevStartDateIndex = 0 - 5;
        var nextStartDateIndex = 0 + 5;
        return (React.createElement("div", { className: "d-flex justify-content-between" },
            React.createElement(react_router_dom_1.Link, { className: 'btn btn-outline-secondary btn-sm', to: "/fetch-data/".concat(prevStartDateIndex) }, "Previous"),
            this.props.isLoading && React.createElement("span", null, "Loading..."),
            React.createElement(react_router_dom_1.Link, { className: 'btn btn-outline-secondary btn-sm', to: "/fetch-data/".concat(nextStartDateIndex) }, "Next")));
    };
    return Recruiter;
}(React.PureComponent));
exports.default = (0, react_redux_1.connect)(function (state) { return state.candidates; }, // Selects which state properties are merged into the component's props
CandidatesStore.actionCreators // Selects which action creators are merged into the component's props
)(Recruiter); // eslint-disable-line @typescript-eslint/no-explicit-any
//# sourceMappingURL=Recruiter.js.map