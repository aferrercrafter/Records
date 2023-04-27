import React, { useState, useEffect } from "react";
import axios from '../axiosInstance';

const Records = () => {
  const [records, setRecords] = useState([]);
  const [newRecord, setNewRecord] = useState({ title: "", description: "" });
  const [updateForm, setUpdateForm] = useState({ id: null, title: "", description: "" });

  useEffect(() => {
    // Fetch records from the API
    // Replace 'your-api-url' with the appropriate API endpoint URL
    axios.get("https://localhost:7007/api/records").then((response) => {
      setRecords(response.data);
    });
  }, []);

  const createRecord = () => {
    axios.post("https://localhost:7007/api/records", newRecord).then((response) => {
      setRecords([...records, response.data]);
    });
  };

  const updateRecord = () => {
    axios.put(`https://localhost:7007/api/records/${updateForm.id}`, updateForm).then((response) => {
        axios.get("https://localhost:7007/api/records").then((response) => {
            setRecords(response.data);
          });
    });
  };

  const deleteRecord = async (recordId) => {
    try {
      await axios.delete(`https://localhost:7007/api/records/${recordId}`);
      axios.get("https://localhost:7007/api/records").then((response) => {
            setRecords(response.data);
          });
    } catch (error) {
      console.error('Error deleting record:', error);
    }
  };

  return (
    <div>
      <h2>Create Record</h2>
      <div>
        <input
          type="text"
          value={newRecord.title}
          onChange={(e) => setNewRecord({ ...newRecord, title: e.target.value })}
        />
        <input
          type="text"
          value={newRecord.description}
          onChange={(e) => setNewRecord({ ...newRecord, description: e.target.value })}
        />
        <button onClick={createRecord}>Create</button>
      </div>

      <h2>Update Record</h2>
      <div>
        <input
          type="text"
          value={updateForm.title}
          onChange={(e) => setUpdateForm({ ...updateForm, title: e.target.value })}
        />
        <input
          type="text"
          value={updateForm.description}
          onChange={(e) => setUpdateForm({ ...updateForm, description: e.target.value })}
        />
        <button onClick={updateRecord} disabled={updateForm.id === null}>
          Update
        </button>
      </div>

      <h2>Records</h2>
      {records.map((record) => (
        <div key={record.id}>
          <div>Title: {record.title}</div>
          <div>Description: {record.description}</div>
          <button
            onClick={() => setUpdateForm({ id: record.id, title: record.title, description: record.description })}
          >
            Edit
          </button>
          <button 
            onClick={() => deleteRecord(record.id)}
          >
            Delete
          </button>
        </div>
      ))}
    </div>
  );
};

export default Records;
