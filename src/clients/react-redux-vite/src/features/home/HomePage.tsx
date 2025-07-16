import { Box } from "@mui/material";
import Sidebar from "../../components/Sidebar";
import Header from "../../components/Header";

const Home = () => {
  return (
    <Box sx={{ display: 'flex' }}>
      <Sidebar />
      <Header />
    </Box>
  )
};

export default Home;