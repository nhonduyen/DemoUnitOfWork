import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CandidatesState {
    isLoading: boolean
    startDateIndex?: number
    candidates: Candidate[]
}

export interface Candidate {
    id: string
    name: string
    recruiterId: string
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestCandidateAction {
    type: 'REQUEST_CANDIDATES'
    startDateIndex: number
}

interface ReceiveCandidateAction {
    type: 'RECEIVE_CANDIDATES',
    startDateIndex: number,
    candidates: Candidate[]
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCandidateAction | ReceiveCandidateAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestCandidates: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.candidates && startDateIndex !== appState.candidates.startDateIndex) {
            fetch(`rest/recruiter/getcandidates`)
                .then(response => response.json() as Promise<Candidate[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_CANDIDATES', startDateIndex: startDateIndex, candidates: data })
                    return
                })

            dispatch({ type: 'REQUEST_CANDIDATES', startDateIndex: startDateIndex })
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: CandidatesState = { candidates: [], isLoading: false };

export const reducer: Reducer<CandidatesState> = (state: CandidatesState | undefined, incomingAction: Action): CandidatesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction
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
