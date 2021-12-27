import * as React from 'react'
import { connect } from 'react-redux'
import { RouteComponentProps } from 'react-router'
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store'
import * as CandidatesStore from '../store/Candidates'

// At runtime, Redux will merge together...
type RecruiterProps =
    CandidatesStore.CandidatesState // ... state we've requested from the Redux store
    & typeof CandidatesStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex }> // ... plus incoming routing parameters


class Recruiter extends React.PureComponent<RecruiterProps> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched()
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched()
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Candidates</h1>
                {this.renderCandidatesTable()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
        this.props.requestCandidates(startDateIndex);
    }

    private renderCandidatesTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Recuiter Id</th>
                    </tr>
                </thead>
                <tbody>
                   {this.props.candidates.map((candidate: CandidatesStore.Candidate) =>
                        <tr key={candidate.id}>
                            <td>{candidate.id}</td>
                            <td>{candidate.name}</td>
                            <td>{candidate.recruiterId}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    private renderPagination() {
        const prevStartDateIndex = 0 - 5;
        const nextStartDateIndex = 0 + 5;

        return (
            <div className="d-flex justify-content-between">
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
                {this.props.isLoading && <span>Loading...</span>}
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.candidates, // Selects which state properties are merged into the component's props
    CandidatesStore.actionCreators // Selects which action creators are merged into the component's props
)(Recruiter as any) // eslint-disable-line @typescript-eslint/no-explicit-any
