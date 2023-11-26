using Unity.Entities;

public partial struct EnemySpawningSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
            state.RequireForUpdate<EnemySpawnerData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        //Query all enemies...
        
    }
}


public partial struct EnemyMoveJob : IJobEntity
{
    
    
    
    public void Execute()
    {
        
    }
}