public abstract class Stage
{
	#region Fields

	protected string stageID = "NULL";

	#endregion

	#region Properties

	public string StageID { get => stageID; }

	#endregion

	#region Methods

	public abstract void StageStart();

	public abstract void StageUISetup();

	public abstract void StageEnd();

	#endregion

}
