using UnityEngine;

public class FarmGameView : MonoBehaviour
{
    private FarmGamePresenter _presenter;

    private void Start()
    {
        _presenter = new FarmGamePresenter(this);
    }

    private void Update()
    {
        _presenter.GameUpdate(Time.deltaTime);
    }

    public virtual void UpdatedGold(int currentGold)
    {

    }
}