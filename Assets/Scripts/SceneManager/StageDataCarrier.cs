public static class StageDataCarrier
{
    // エラーの原因：変数名を「StageType」にしてしまうと衝突します。
    // 対策：変数名を「SelectedStageType」などの別名にします。
    public static StageType SelectedStageType = StageType.Square;
}
