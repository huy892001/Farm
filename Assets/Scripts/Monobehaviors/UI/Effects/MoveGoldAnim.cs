using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveGoldAnim : PersistentSingleton<MoveGoldAnim>
{
    public GameObject objFlyPrefab;
    public GameObject fxLightStartPrefab;
    public Transform targetTransform;
    public float stage1Duration = .3f;
    public float stage2Duration = .4f;
    public float delayTime = .02f;
    public int numAcorns = 20;
    public float innerRadius = 2f;
    public float outerRadius = 4f;
    public int numAcornInAPhase = 6;
    public float decreaseTimeAmountAfterEachPhase = .02f;
    public float minScale = .4f;
    public float maxScale = 1.0f;

    [Space]
    [Header("Debug params")]
    public float defaultDelayTime = .1f;
    float curStage1Duration;
    float curStage2Duration;

    //public float defaultDelayTime = .1f;
    //public float stage1 = .1f;
    //public float stage2 = .1f;
    float curDelayTime;

    private void Start()
    {
        curStage1Duration = stage1Duration;
        curStage2Duration = stage2Duration;
    }

    public void BtnPlay2()
    {
        // MoveAcorn(transform.position, targetTransform.position, numAcorns);
    }

    public void MoveGolds(Vector3 fromPosition, Vector3 toPosition, int numAcornsPram, Action onMoveItemEnded, Action onMoveItemsDone, Action onFirstItemMoveDone)
    {
        StartCoroutine(Play(fromPosition, toPosition, numAcornsPram, onMoveItemEnded, onMoveItemsDone, onFirstItemMoveDone, null, (GameObject goldFly) =>
        {
            if (goldFly && goldFly.GetComponent<ParticleSystem>())
            {
                goldFly.GetComponent<ParticleSystem>().Play();
            }
        }));
    }

    // public void MoveAcorn(Vector3 fromPosition, Vector3 toPosition, int numAcornsPram, Action itemCallback = null, Action callBack = null, Action onItemInited = null, Action<GameObject> onItemStage1Ended = null)
    // {
    //     StartCoroutine(Play(fromPosition, toPosition, numAcornsPram, itemCallback, callBack, onItemInited, onItemStage1Ended));
    // }

    public IEnumerator Play(Vector3 fromPosition, Vector3 toPosition, int numAcornsParam, Action itemCallback = null, Action callBack = null, 
    Action onFirstItemMoveDone = null, Action onItemInited = null, Action<GameObject> onItemStage1Ended = null)
    {
        fromPosition.z = 0;
        toPosition.z = 0;
        curDelayTime = delayTime;
        numAcorns = numAcornsParam;
        curStage1Duration = stage1Duration;
        curStage2Duration = stage2Duration;
        int acornCnt = 0;

        GameObject fxLightInstance = Instantiate(fxLightStartPrefab, transform);
        fxLightInstance.transform.position = fromPosition;
        fxLightInstance.transform.localScale = Vector3.one * .5f;

        List<Vector3> randomAcornPositions = new List<Vector3>();
        GenerateRandomAcornPositions(fromPosition, randomAcornPositions);
        bool firstItemMoveDone = false;

        for (int i = 0; i < numAcorns; i++)
        {
            fxLightInstance.transform.DOScale(.5f + (i / (float)numAcorns) * .5f, curDelayTime);
            int curI = i;
            bool isLastStage = false;
            int totalAcornsInCurrentPhase = numAcornInAPhase;
            if ((curI + 1) > (numAcorns / numAcornInAPhase) * numAcornInAPhase)
            {
                totalAcornsInCurrentPhase = numAcorns % numAcornInAPhase;
                isLastStage = true;
            }

            //float angleOffset = 2 * Mathf.PI / totalAcornsInCurrentPhase;
            //float rdAngleOffset = UnityEngine.Random.Range(-angleOffset / 4.0f, angleOffset / 4.0f);
            //float angle = (i % numAcornInAPhase) * angleOffset + rdAngleOffset;
            GameObject flyAcornInstance = SpawnFlyAcorn(fromPosition);
            Vector3 outerTargetPos = randomAcornPositions[i];//GetOuterTargetPosition(fromPosition, angle);
            onItemInited?.Invoke();

            flyAcornInstance.transform.DOScale(UnityEngine.Random.Range(minScale, maxScale), curStage1Duration);
            flyAcornInstance.transform.DOMove(outerTargetPos, curStage1Duration).SetEase(Ease.OutSine).OnComplete(() =>
            {
                onItemStage1Ended?.Invoke(flyAcornInstance.gameObject);
                if (curI == numAcorns - 1)
                {
                    fxLightInstance.GetComponent<Image>().DOFade(.0f, stage2Duration / 2).OnComplete(() =>
                    {
                        Destroy(fxLightInstance.gameObject);
                    });
                }
                Sequence sq = DOTween.Sequence()
                .Append(flyAcornInstance.transform.DOMove(toPosition, curStage2Duration).SetEase(Ease.InSine))
                //.Join(flyAcornInstance.transform.DOScale(.4f, curStage2Duration))
                //.SetDelay(.2f * delayTime * (totalAcornsInCurrentPhase - (curI + 1) % totalAcornsInCurrentPhase))
                .OnComplete(() =>
                {
                    if (!firstItemMoveDone) {
                        firstItemMoveDone = true;
                        onFirstItemMoveDone?.Invoke();
                    }
                    itemCallback?.Invoke();
                    if (curI == numAcorns - 1)
                    {
                        callBack?.Invoke();
                    }
                    if (flyAcornInstance)
                    {
                        Destroy(flyAcornInstance);
                    }
                    //if (flyAcornInstance.GetComponent<IMoveableObject>() != null)
                    //{
                    //    flyAcornInstance.GetComponent<IMoveableObject>().OnMoveComplete();
                    //}
                });
                if (!isLastStage)
                {
                    sq.SetDelay(.2f * curDelayTime * (totalAcornsInCurrentPhase - (curI + 1) % totalAcornsInCurrentPhase));
                }
                sq.Play();
            });
            curDelayTime = Mathf.Max(.01f, curDelayTime * .95f);

            if (i < numAcorns - 1)
            {
                yield return new WaitForSeconds(curDelayTime);
                acornCnt++;
                if (acornCnt % numAcornInAPhase == 0)
                {
                    curStage1Duration = Mathf.Max(.01f, curStage1Duration - decreaseTimeAmountAfterEachPhase);
                    curStage2Duration = Mathf.Max(.01f, curStage2Duration - decreaseTimeAmountAfterEachPhase);
                }
            }
        }
    }

    private void GenerateRandomAcornPositions(Vector3 fromPosition, List<Vector3> randomAcornPositions)
    {
        for (int i = 0; i < numAcorns; i++)
        {
            int totalAcornsInCurrentPhase = numAcornInAPhase;
            if ((i + 1) > (numAcorns / numAcornInAPhase) * numAcornInAPhase)
            {
                totalAcornsInCurrentPhase = numAcorns % numAcornInAPhase;
            }
            float angleOffset = 2 * Mathf.PI / totalAcornsInCurrentPhase;
            float rdAngleOffset = UnityEngine.Random.Range(-angleOffset / 4.0f, angleOffset / 4.0f);
            float angle = (i % numAcornInAPhase) * angleOffset + rdAngleOffset;
            Vector3 outerTargetPos = GetOuterTargetPosition(fromPosition, angle);
            randomAcornPositions.Add(outerTargetPos);
        }
        for (int i = 0; i < numAcorns; i += numAcornInAPhase)
        {
            CUtils.Shuffle<Vector3>(randomAcornPositions, i, Mathf.Min(i + numAcorns - 1, numAcorns - 1));
        }
    }

    private Vector3 GetOuterTargetPosition(Vector3 startPosition, float rdAngle)
    {
        Vector3 outerTargetPos = startPosition + innerRadius * new Vector3(Mathf.Cos(rdAngle), Mathf.Sin(rdAngle), 0f);
        Vector3 toOuterTargetDirection = outerTargetPos - startPosition;
        toOuterTargetDirection.Normalize();
        outerTargetPos += toOuterTargetDirection * UnityEngine.Random.Range(0, outerRadius - innerRadius);
        return outerTargetPos;
    }

    private GameObject SpawnFlyAcorn(Vector3 fromPosition)
    {
        GameObject flyAcornInstance = Instantiate(objFlyPrefab);//pooler.GetPooledObject();
        flyAcornInstance.transform.SetParent(transform);
        flyAcornInstance.transform.position = fromPosition;
        flyAcornInstance.transform.localScale = Vector3.zero;
        return flyAcornInstance;
    }

    //private void SpawnFX(Vector3 targetPosition, FXType fxType)
    //{
    //    Transform fxLightInstance = fxPooler.GetPooledObject((int)fxType).transform;
    //    fxLightInstance.SetParent(transform);
    //    fxLightInstance.localScale = Vector3.one;
    //    Image fxImg = fxLightInstance.GetComponent<Image>();
    //    fxImg.transform.position = targetPosition;
    //    fxImg.DOFade(.0f, .0f);
    //    fxImg.DOFade(1.0f, .15f).OnComplete(() =>
    //    {
    //        fxPooler.Push(fxImg.gameObject, (int)fxType);
    //    });
    //}



#if UNITY_EDITOR
    //public void BtnPlay()
    //{
    //    StartCoroutine(Play(null));
    //}

    //public IEnumerator Play(Action callback)
    //{
    //    float angleOffset = 2 * Mathf.PI / Mathf.Min(numAcorns, numAcornInAPhase);
    //    float curAngle = 0f;
    //    int acornCnt = 0;
    //    Transform fxLightInstance = Instantiate<Transform>(fxLightStartPrefab.transform, transform);
    //    fxLightInstance.localPosition = Vector3.zero;
    //    fxLightInstance.transform.localScale = Vector3.one * 2.0f;
    //    Color fxLightColor = fxLightInstance.GetComponent<Image>().color;
    //    fxLightColor.a = .5f;
    //    fxLightInstance.GetComponent<Image>().color = fxLightColor;
    //    for (int i = 0; i < numAcorns; i++)
    //    {
    //        fxLightInstance.GetComponent<Image>().DOFade(.5f + .5f * (i / (float)numAcorns), .1f);
    //        GameObject flyAcornInstance = Instantiate(flyAcornPrefab, transform);
    //        flyAcornInstance.transform.localPosition = Vector3.zero;
    //        flyAcornInstance.transform.localScale = Vector3.zero;
    //        float rdAngle = UnityEngine.Random.Range(curAngle - curAngle / 4.0f, curAngle + curAngle / 4.0f);
    //        Vector3 outerTargetPos = flyAcornInstance.transform.position + innerRadius * new Vector3(Mathf.Cos(rdAngle), Mathf.Sin(rdAngle), 0f);
    //        Vector3 toOuterTargetDirection = outerTargetPos - transform.position;
    //        toOuterTargetDirection.Normalize();
    //        outerTargetPos += toOuterTargetDirection * UnityEngine.Random.Range(0, outerRadius - innerRadius);
    //        curAngle += angleOffset;
    //Debug.LogError("From position: " + fromPosition);
    //Debug.LogError("To position: " + toPosition);
    //        flyAcornInstance.transform.DOScale(UnityEngine.Random.Range(.6f, 1.0f), stage1Duration);
    //        flyAcornInstance.transform.DOMove(outerTargetPos, stage1Duration).SetEase(Ease.OutSine).OnComplete(() =>
    //        {
    //            flyAcornInstance.transform.DOScale(.5f, stage2Duration);
    //            flyAcornInstance.transform.DOMove(targetTransform.position, stage2Duration).SetEase(Ease.InSine).OnComplete(() =>
    //            {
    //                Destroy(flyAcornInstance);
    //            });
    //        });

    //        int curI = i;
    //        if (curI == numAcorns - 1)
    //        {
    //            fxLightInstance.GetComponent<Image>().DOFade(.0f, .1f).OnComplete(() =>
    //            {
    //                Destroy(fxLightInstance.gameObject);
    //            });
    //        }
    //        yield return new WaitForSeconds(delayTime);
    //        acornCnt++;
    //        if (acornCnt % numAcornInAPhase == 0)
    //        {
    //            stage1Duration = Mathf.Max(.1f, stage1Duration - decreaseTimeAmountAfterEachPhase);
    //            stage2Duration = Mathf.Max(.1f, stage2Duration - decreaseTimeAmountAfterEachPhase);
    //            yield return null;
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
#endif
}
