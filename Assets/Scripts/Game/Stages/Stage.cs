public abstract class Stage {

    #region Properties

    protected string stageID = "NULL";

    #endregion

    #region Methods

    public string GetStageID() {
        return stageID;
    }

    public abstract void StageStart();

    public abstract void StageUISetup();

    public abstract void StageEnd();

    #endregion

}
