"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducer = exports.actionCreators = void 0;
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    requestCandidates: function (startDateIndex) { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.candidates && startDateIndex !== appState.candidates.startDateIndex) {
            fetch("rest/recruiter/getcandidates")
                .then(function (response) { return response.json(); })
                .then(function (data) {
                dispatch({ type: 'RECEIVE_CANDIDATES', startDateIndex: startDateIndex, candidates: data });
                return;
            });
            dispatch({ type: 'REQUEST_CANDIDATES', startDateIndex: startDateIndex });
        }
    }; }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var unloadedState = { candidates: [], isLoading: false };
var reducer = function (state, incomingAction) {
    if (state === undefined) {
        return unloadedState;
    }
    var action = incomingAction;
    switch (action.type) {
        case 'REQUEST_CANDIDATES':
            return {
                candidates: state.candidates,
                startDateIndex: action.startDateIndex,
                isLoading: true
            };
        case 'RECEIVE_CANDIDATES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex !== state.startDateIndex) {
                return {
                    candidates: state.candidates,
                    isLoading: false
                };
            }
            break;
    }
    return state;
};
exports.reducer = reducer;
//# sourceMappingURL=Candidates.js.map