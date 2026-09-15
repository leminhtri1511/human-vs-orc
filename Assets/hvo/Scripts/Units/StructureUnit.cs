using HVO.Scripts.Utils;

namespace HVO.Scripts.Units
{
    public class StructureUnit : Unit
    {
        private BuildingProcess _buildingProcess;
        public bool IsUnderConstruction => _buildingProcess != null;

        private void Update()
        {
            if (IsUnderConstruction)
            {
                _buildingProcess.Update();
            }
        }

        public void RegisterProcess(BuildingProcess process)
        {
            _buildingProcess = process;
        }
    }
}