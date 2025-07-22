import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { schoolContextService, UserSchool } from "../../services/schoolContextService";

const SchoolSelector = () => {
  const [schools, setSchools] = useState<UserSchool[]>([]);
  const [selectedSchoolId, setSelectedSchoolId] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const initializeSchools = async () => {
      try {
        await schoolContextService.initialize();
        const userSchools = schoolContextService.getUserSchools();
        setSchools(userSchools);

        // If user only has one school, redirect automatically
        if (userSchools.length === 1) {
          await schoolContextService.switchSchool(userSchools[0].schoolId);
          navigate("/home");
          return;
        }

        // If there's a current school, pre-select it
        const currentSchool = schoolContextService.getCurrentSchool();
        if (currentSchool) {
          setSelectedSchoolId(currentSchool.schoolId);
        }
      } catch (error) {
        console.error("Failed to initialize schools:", error);
      } finally {
        setIsLoading(false);
      }
    };

    initializeSchools();
  }, [navigate]);

  const handleSchoolSelect = async () => {
    if (!selectedSchoolId) {
      alert("Please select a school to continue.");
      return;
    }

    try {
      await schoolContextService.switchSchool(selectedSchoolId);
      navigate("/home");
    } catch (error) {
      console.error("Failed to switch school context:", error);
      alert("Failed to select school. Please try again.");
    }
  };

  if (isLoading) {
    return (
      <div style={{ padding: "2rem", textAlign: "center" }}>
        <h2>Loading your schools...</h2>
      </div>
    );
  }

  if (schools.length === 0) {
    return (
      <div style={{ padding: "2rem", textAlign: "center" }}>
        <h2>No Schools Found</h2>
        <p>You don't appear to have access to any schools yet. Please contact your school administrator.</p>
      </div>
    );
  }

  return (
    <div style={{ padding: "2rem", maxWidth: "600px", margin: "0 auto" }}>
      <h2>Select Your School Context</h2>
      <p>You have access to multiple schools. Please select which school you'd like to work with.</p>
      
      <div style={{ marginBottom: "2rem" }}>
        {schools.map(school => (
          <div 
            key={school.schoolId} 
            style={{ 
              border: selectedSchoolId === school.schoolId ? "2px solid #007acc" : "1px solid #ccc",
              borderRadius: "8px",
              padding: "1rem",
              margin: "0.5rem 0",
              cursor: "pointer",
              backgroundColor: selectedSchoolId === school.schoolId ? "#f0f8ff" : "white"
            }}
            onClick={() => setSelectedSchoolId(school.schoolId)}
          >
            <div style={{ display: "flex", alignItems: "center" }}>
              <input
                type="radio"
                name="school"
                value={school.schoolId}
                checked={selectedSchoolId === school.schoolId}
                onChange={() => setSelectedSchoolId(school.schoolId)}
                style={{ marginRight: "1rem" }}
              />
              <div>
                <h3 style={{ margin: "0 0 0.5rem 0" }}>{school.schoolName}</h3>
                <p style={{ margin: "0", color: "#666", fontSize: "0.9rem" }}>
                  Role: {school.roles.join(', ')} | Children: {school.children.map(child => child.name).join(", ")}
                </p>
              </div>
            </div>
          </div>
        ))}
      </div>

      <button 
        onClick={handleSchoolSelect}
        disabled={!selectedSchoolId}
        style={{ 
          width: "100%", 
          padding: "0.75rem", 
          fontSize: "1rem",
          backgroundColor: selectedSchoolId ? "#007acc" : "#ccc",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: selectedSchoolId ? "pointer" : "not-allowed"
        }}
      >
        Continue to {schools.find(s => s.schoolId === selectedSchoolId)?.schoolName || "Selected School"}
      </button>
    </div>
  );
};

export default SchoolSelector;